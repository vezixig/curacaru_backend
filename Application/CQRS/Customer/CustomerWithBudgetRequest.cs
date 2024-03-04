namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get a customer with its budget.</summary>
public class CustomerWithBudgetRequest(Guid companyId, string authId, Guid customerId) : IRequest<GetCustomerBudgetDto>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;
}

internal class CustomerWithBudgetRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<CustomerWithBudgetRequest, GetCustomerBudgetDto>
{
    public async Task<GetCustomerBudgetDto> Handle(CustomerWithBudgetRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var isReplacement = await appointmentRepository.IsAppointmentReplacement(request.CustomerId, employee!.Id);

        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId, employee!.IsManager || isReplacement ? null : employee.Id)
                       ?? throw new NotFoundException("Kunde existiert nicht");

        var budget = await budgetRepository.GetCurrentBudgetAsync(request.CompanyId, request.CustomerId) ?? new Budget { Customer = customer };

        var result = mapper.Map<GetCustomerBudgetDto>(customer);
        result.CareBenefitAmount = budget.CareBenefitAmount;
        result.PreventiveCareAmount = budget.PreventiveCareAmount;
        result.ReliefAmount = budget.ReliefAmount + budget.ReliefAmountLastYear;
        result.SelfPayAmount = budget.SelfPayAmount;

        return result;
    }
}