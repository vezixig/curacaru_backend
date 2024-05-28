namespace Curacaru.Backend.Application.CQRS.Customer;

using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to delete a customer.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="customerId">The id of the customer to delete.</param>
/// <param name="deleteOpenAppointments">A value indicating whether all open appointments should also be deleted.</param>
/// <param name="deleteBudgets">A value indicating whether the budgets of the customer should be deleted.</param>
public class SetCustomerInactiveRequest(
    User user,
    Guid customerId,
    bool deleteOpenAppointments,
    bool deleteBudgets) : IRequest
{
    public Guid CustomerId { get; } = customerId;

    public bool DeleteBudgets { get; } = deleteBudgets;

    public bool DeleteOpenAppointments { get; } = deleteOpenAppointments;

    public User User { get; } = user;
}

internal class SetCustomerInactiveRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService) : IRequestHandler<SetCustomerInactiveRequest>
{
    public async Task Handle(SetCustomerInactiveRequest inactiveRequest, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(inactiveRequest.User.CompanyId, inactiveRequest.CustomerId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            customer.Status = CustomerStatus.Former;
            await customerRepository.UpdateCustomerAsync(customer);

            if (inactiveRequest.DeleteOpenAppointments)
            {
                var appointments = await appointmentRepository.GetAppointmentsAsync(
                    inactiveRequest.User.CompanyId,
                    customerId: inactiveRequest.CustomerId,
                    employeeId: null,
                    from: null,
                    to: null,
                    onlyOpen: true,
                    asTracking: true);
                await appointmentRepository.DeleteAppointmentsAsync(appointments);
            }

            if (inactiveRequest.DeleteBudgets)
            {
                var budgets = await budgetRepository.GetCurrentBudgetAsync(
                    inactiveRequest.User.CompanyId,
                    inactiveRequest.CustomerId);
                if (budgets != null)
                {
                    budgets.CareBenefitAmount = 0;
                    budgets.PreventiveCareAmount = 0;
                    budgets.ReliefAmount = 0;
                    budgets.ReliefAmountLastYear = 0;
                    budgets.SelfPayAmount = 0;
                    budgets.SelfPayRaise = 0;
                    await budgetRepository.UpdateBudgetAsync(budgets);
                }
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}