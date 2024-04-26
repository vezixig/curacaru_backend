namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get a customer with its budget.</summary>
public class CustomerWithBudgetRequest(User user, Guid customerId) : IRequest<GetCustomerBudgetDto>
{
    public Guid CustomerId { get; } = customerId;

    public User User { get; } = user;
}

internal class CustomerWithBudgetRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    ICustomerRepository customerRepository,
    IMapper mapper)
    : IRequestHandler<CustomerWithBudgetRequest, GetCustomerBudgetDto>
{
    public async Task<GetCustomerBudgetDto> Handle(CustomerWithBudgetRequest request, CancellationToken cancellationToken)
    {
        var isReplacement = await appointmentRepository.IsAppointmentReplacement(request.CustomerId, request.User.EmployeeId);

        var customer = await customerRepository.GetCustomerAsync(
                           request.User.CompanyId,
                           request.CustomerId,
                           request.User.IsManager || isReplacement ? null : request.User.EmployeeId)
                       ?? throw new NotFoundException("Kunde existiert nicht");

        var budget = await budgetRepository.GetCurrentBudgetAsync(request.User.CompanyId, request.CustomerId) ?? new Budget { Customer = customer };

        var result = mapper.Map<GetCustomerBudgetDto>(customer);
        result.CareBenefitAmount = budget.CareBenefitAmount;
        result.PreventiveCareAmount = budget.PreventiveCareAmount;
        result.ReliefAmount = budget.ReliefAmount + budget.ReliefAmountLastYear;
        result.SelfPayAmount = budget.SelfPayAmount;

        return result;
    }
}