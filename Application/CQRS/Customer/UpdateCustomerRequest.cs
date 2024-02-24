namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class UpdateCustomerRequest(UpdateCustomerDto customerData, Guid companyId, string authId) : IRequest<GetCustomerDto>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public UpdateCustomerDto CustomerData { get; } = customerData;
}

public class UpdateCustomerRequestHandler(
    IBudgetRepository budgetRepository,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<UpdateCustomerRequest, GetCustomerDto>
{
    public async Task<GetCustomerDto> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerData.Id)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (customer.CompanyId != request.CompanyId) throw new ForbiddenException("Sie dürfen diesen Kunden nicht bearbeiten.");

        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            _ = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.CustomerData.AssociatedEmployeeId.Value)
                ?? throw new BadRequestException("Bearbeitenden Mitarbeiter nicht gefunden.");

        // Remove clearance amounts from budgets if not allowed
        var budget = await budgetRepository.GetCurrentBudgetAsync(request.CompanyId, request.CustomerData.Id);

        if (budget is not null)
        {
            budget.ReliefAmount = request.CustomerData.DoClearanceReliefAmount ? budget.ReliefAmount : 0;
            budget.ReliefAmountLastYear = request.CustomerData.DoClearanceReliefAmount ? budget.ReliefAmountLastYear : 0;
            budget.CareBenefitAmount = request.CustomerData.DoClearanceCareBenefit ? budget.CareBenefitAmount : 0;
            budget.PreventiveCareAmount = request.CustomerData.DoClearancePreventiveCare ? budget.PreventiveCareAmount : 0;
            budget.SelfPayAmount = request.CustomerData.DoClearanceSelfPayment ? budget.SelfPayAmount : 0;
        }

        mapper.Map(request.CustomerData, customer);
        customer.AssociatedEmployee = new Employee { Id = request.CustomerData.AssociatedEmployeeId!.Value };
        customer.Insurance = request.CustomerData.InsuranceId.HasValue ? new Insurance { Id = request.CustomerData.InsuranceId.Value } : null;
        customer.ZipCity = new ZipCity { ZipCode = request.CustomerData.ZipCode! };

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            var updatedCustomer = await customerRepository.UpdateCustomerAsync(customer);
            if (budget is not null) await budgetRepository.UpdateBudgetAsync(budget);

            await transaction.CommitAsync(cancellationToken);
            return mapper.Map<GetCustomerDto>(updatedCustomer);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}