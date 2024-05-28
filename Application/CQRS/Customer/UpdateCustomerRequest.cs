namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class UpdateCustomerRequest(User user, UpdateCustomerDto customerData) : IRequest<GetCustomerDto>
{
    public UpdateCustomerDto CustomerData { get; } = customerData;

    public User User { get; } = user;
}

public class UpdateCustomerRequestHandler(
    IBudgetRepository budgetRepository,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService,
    IEmployeeRepository employeeRepository,
    IInsuranceRepository insuranceRepository,
    IMapper mapper,
    IProductRepository productRepository,
    IAddressRepository addressRepository)
    : IRequestHandler<UpdateCustomerRequest, GetCustomerDto>
{
    public async Task<GetCustomerDto> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.CustomerData.Id, asTracking: true)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (customer.CompanyId != request.User.CompanyId) throw new ForbiddenException("Sie dürfen diesen Kunden nicht bearbeiten.");

        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            _ = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.CustomerData.AssociatedEmployeeId.Value)
                ?? throw new BadRequestException("Bearbeitenden Mitarbeiter nicht gefunden.");

        // validate products
        var products = await productRepository.GetProducts(request.CustomerData.Products);
        if (products.Count != request.CustomerData.Products.Count) throw new BadRequestException("Ein oder mehrere Produkte wurden nicht gefunden.");

        customer.Products.RemoveAll(o => !request.CustomerData.Products.Contains(o.Id));
        customer.Products.AddRange(products.Where(o => customer.Products.TrueForAll(p => p.Id != o.Id)));

        // validate zip code
        var zipCity = await addressRepository.GetZipCityAsync(request.CustomerData.ZipCode!)
                      ?? throw new BadRequestException("Postleitzahl nicht gefunden.");
        customer.ZipCity = zipCity;

        // validate insurance
        var insurance = request.CustomerData.InsuranceId.HasValue
            ? await insuranceRepository.GetInsuranceAsync(request.User.CompanyId, request.CustomerData.InsuranceId.Value, true)
              ?? throw new BadRequestException("Versicherung nicht gefunden.")
            : null;
        customer.Insurance = insurance;

        // Remove clearance amounts from budgets if not allowed
        var budget = await budgetRepository.GetCurrentBudgetAsync(request.User.CompanyId, request.CustomerData.Id);

        if (budget is not null)
        {
            budget.Customer = customer;

            budget.ReliefAmount = request.CustomerData.DoClearanceReliefAmount ? budget.ReliefAmount : 0;
            budget.ReliefAmountLastYear = request.CustomerData.DoClearanceReliefAmount ? budget.ReliefAmountLastYear : 0;
            budget.CareBenefitAmount = request.CustomerData.DoClearanceCareBenefit ? budget.CareBenefitAmount : 0;
            budget.PreventiveCareAmount = request.CustomerData.DoClearancePreventiveCare ? budget.PreventiveCareAmount : 0;
            budget.SelfPayAmount = request.CustomerData.DoClearanceSelfPayment ? budget.SelfPayAmount : 0;
        }

        mapper.Map(request.CustomerData, customer);
        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            customer.AssociatedEmployee = new() { Id = request.CustomerData.AssociatedEmployeeId!.Value };
        else
            customer.AssociatedEmployee = null;

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            var updatedCustomer = await customerRepository.UpdateCustomerAsync(customer);
            if (budget is not null) await budgetRepository.UpdateBudgetAsync(budget);
            await transaction.CommitAsync(cancellationToken);

            return mapper.Map<GetCustomerDto>(updatedCustomer);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}