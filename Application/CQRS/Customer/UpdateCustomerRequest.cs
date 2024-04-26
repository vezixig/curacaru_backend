namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
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
    IMapper mapper)
    : IRequestHandler<UpdateCustomerRequest, GetCustomerDto>
{
    public async Task<GetCustomerDto> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.CustomerData.Id)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (customer.CompanyId != request.User.CompanyId) throw new ForbiddenException("Sie dürfen diesen Kunden nicht bearbeiten.");

        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            _ = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.CustomerData.AssociatedEmployeeId.Value)
                ?? throw new BadRequestException("Bearbeitenden Mitarbeiter nicht gefunden.");

        // Remove clearance amounts from budgets if not allowed
        var budget = await budgetRepository.GetCurrentBudgetAsync(request.User.CompanyId, request.CustomerData.Id);

        if (budget is not null)
        {
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

        customer.Insurance = request.CustomerData.InsuranceId.HasValue ? new Insurance { Id = request.CustomerData.InsuranceId.Value } : null;
        customer.ZipCity = new() { ZipCode = request.CustomerData.ZipCode! };

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