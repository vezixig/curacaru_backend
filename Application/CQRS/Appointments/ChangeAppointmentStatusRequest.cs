namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;
using Services;

/// <summary>Request to change the status of an appointment.</summary>
public class ChangeAppointmentStatusRequest(
    User user,
    Guid appointmentId,
    bool isDone) : IRequest<GetAppointmentListEntryDto>
{
    /// <summary>Gets the appointment id.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets a value indicating whether the appointment should be marked as done.</summary>
    public bool IsDone { get; } = isDone;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class FinishAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IDateTimeService dateTimeService,
    IMapper mapper)
    : IRequestHandler<ChangeAppointmentStatusRequest, GetAppointmentListEntryDto>
{
    public async Task<GetAppointmentListEntryDto> Handle(ChangeAppointmentStatusRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.User.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.Date > dateTimeService.Today && request.IsDone) throw new BadRequestException("Termine in der Zukunft können nicht abgeschlossen werden.");

        if (appointment.Date < dateTimeService.BeginOfCurrentMonth && !request.IsDone)
            throw new BadRequestException("Termine vor dem aktuellen Monat können nicht wieder geöffnet werden.");

        if (string.IsNullOrEmpty(appointment.SignatureEmployee) || string.IsNullOrEmpty(appointment.SignatureCustomer))
            throw new BadRequestException("Der Termin wurde noch nicht von Kunden und Mitarbeiter unterschrieben.");

        if (request.User.EmployeeId != appointment.EmployeeId && request.User.EmployeeId != appointment.EmployeeReplacementId && !request.User.IsManager)
            throw new ForbiddenException("Nur Manager dürfen den Status fremder Termine ändern.");

        if (request is { IsDone: false, User.IsManager: false }) throw new ForbiddenException("Nur Manager dürfen Termine wieder öffnen.");

        if (!request.IsDone && appointment.DeploymentReportId.HasValue)
            throw new ForbiddenException("Der Termin wurde bereits in einem Einsatznachweis erfasst und darf nicht wieder geöffnet werden.");

        appointment.IsDone = request.IsDone;
        appointment.Employee = new() { Id = appointment.EmployeeId };
        appointment.EmployeeReplacement = appointment.EmployeeReplacementId.HasValue ? new Employee { Id = appointment.EmployeeReplacementId!.Value } : null;
        appointment.Customer = new() { Id = appointment.CustomerId };
        if (!request.IsDone)
        {
            appointment.SignatureEmployee = "";
            appointment.SignatureCustomer = "";
        }

        await appointmentRepository.UpdateAppointmentAsync(appointment);

        return mapper.Map<GetAppointmentListEntryDto>(appointment);
    }
}