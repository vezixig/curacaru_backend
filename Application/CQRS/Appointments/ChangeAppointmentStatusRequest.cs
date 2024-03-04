namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;
using Services;

public class ChangeAppointmentStatusRequest(
    Guid companyId,
    string authId,
    Guid appointmentId,
    bool isDone) : IRequest<GetAppointmentListEntryDto>
{
    public Guid AppointmentId { get; } = appointmentId;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public bool IsDone { get; } = isDone;
}

internal class FinishAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IDateTimeService dateTimeService,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<ChangeAppointmentStatusRequest, GetAppointmentListEntryDto>
{
    public async Task<GetAppointmentListEntryDto> Handle(ChangeAppointmentStatusRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.Date > dateTimeService.Today && request.IsDone) throw new BadRequestException("Termine in der Zukunft können nicht abgeschlossen werden.");

        if (appointment.Date < dateTimeService.BeginOfCurrentMonth && !request.IsDone)
            throw new BadRequestException("Termine vor dem aktuellen Monat können nicht wieder geöffnet werden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != appointment.EmployeeId && user.Id != appointment.EmployeeReplacementId && !user.IsManager)
            throw new ForbiddenException("Nur Manager dürfen den Status fremder Termine ändern.");

        if (!request.IsDone && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen Termine wieder öffnen.");

        appointment.IsDone = request.IsDone;
        appointment.Employee = new() { Id = appointment.EmployeeId };
        appointment.EmployeeReplacement = appointment.EmployeeReplacementId.HasValue ? new Employee { Id = appointment.EmployeeReplacementId!.Value } : null;
        appointment.Customer = new() { Id = appointment.CustomerId };

        await appointmentRepository.UpdateAppointmentAsync(appointment);

        return mapper.Map<GetAppointmentListEntryDto>(appointment);
    }
}