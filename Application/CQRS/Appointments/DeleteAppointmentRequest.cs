namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Services;

/// <summary>Request to delete an appointment.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="authId">The auth id of the user.</param>
/// <param name="appointmentId">The appointment id.</param>
public class BudgetService(Guid companyId, string authId, Guid appointmentId) : IRequest
{
    public Guid AppointmentId { get; } = appointmentId;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class DeleteAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetService budgetService,
    IDatabaseService databaseService,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<BudgetService>
{
    public async Task Handle(BudgetService request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine löschen.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            await budgetService.RefundBudget(appointment);

            await appointmentRepository.DeleteAppointmentAsync(appointment);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}