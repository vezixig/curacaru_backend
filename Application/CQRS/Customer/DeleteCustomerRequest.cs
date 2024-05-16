namespace Curacaru.Backend.Application.CQRS.Customer;

using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to delete a customer.</summary>
/// <param name="authId">The auth id of the user.</param>
/// <param name="customerId">The id of the customer to delete.</param>
/// <param name="deleteOpenAppointments">A value indicating whether all open appointments should also be deleted.</param>
public class DeleteCustomerRequest(User user, Guid customerId, bool deleteOpenAppointments) : IRequest
{
    public Guid CustomerId { get; } = customerId;

    public bool DeleteOpenAppointments { get; } = deleteOpenAppointments;

    public User User { get; } = user;
}

internal class DeleteCustomerRequestHandler(
    IAppointmentRepository appointmentRepository,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService,
    IEmployeeRepository employeeRepository) : IRequestHandler<DeleteCustomerRequest>
{
    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.CustomerId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            customer.Status = CustomerStatus.Former;
            await customerRepository.UpdateCustomerAsync(customer);

            if (request.DeleteOpenAppointments)
            {
                var appointments = await appointmentRepository.GetAppointmentsAsync(
                    request.User.CompanyId,
                    customerId: request.CustomerId,
                    employeeId: null,
                    from: null,
                    to: null,
                    onlyOpen: true,
                    asTracking: true);
                await appointmentRepository.DeleteAppointmentsAsync(appointments);
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