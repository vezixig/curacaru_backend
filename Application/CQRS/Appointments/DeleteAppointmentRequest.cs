namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Services;

/// <summary>Request to delete an appointment.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="appointmentId">The appointment id.</param>
public class DeleteAppointmentRequest(User user, Guid appointmentId) : IRequest
{
    /// <summary>Gets the appointment id.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class DeleteAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetService budgetService,
    IDatabaseService databaseService,
    IDateTimeService dateTimeService)
    : IRequestHandler<DeleteAppointmentRequest>
{
    public async Task Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.User.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.IsDone) throw new BadRequestException("Abgeschlossene Termine können nicht gelöscht werden.");

        if (appointment.Date < dateTimeService.BeginOfCurrentMonth && !appointment.IsPlanned)
            throw new BadRequestException("Termine vor dem aktuellen Monat können nicht gelöscht werden.");

        if (request.User.EmployeeId != appointment.EmployeeId && !request.User.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine löschen.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            if (appointment is { IsPlanned: false, HasBudgetError: false }) await budgetService.RefundBudget(appointment);

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