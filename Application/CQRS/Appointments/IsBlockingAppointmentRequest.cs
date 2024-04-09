namespace Curacaru.Backend.Application.CQRS.Appointments;

using Infrastructure.Repositories;
using MediatR;

/// <summary>Checks if an appointment would block another appointment.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="employeeId">The employee id.</param>
/// <param name="date">The date of the appointment.</param>
/// <param name="start">The start time of the appointment.</param>
/// <param name="end">The end time of the appointment.</param>
/// <param name="appointmentId">The id of appointment if it already exists to ignore itself.</param>
public class IsBlockingAppointmentRequest(
    Guid companyId,
    Guid employeeId,
    DateOnly date,
    TimeOnly start,
    TimeOnly end,
    Guid? appointmentId) : IRequest<bool>
{
    public Guid? AppointmentId { get; } = appointmentId;

    public Guid CompanyId { get; } = companyId;

    public DateOnly Date { get; } = date;

    public Guid EmployeeId { get; } = employeeId;

    public TimeOnly End { get; } = end;

    public TimeOnly Start { get; } = start;
}

public class IsBlockingAppointmentRequestHandler(IAppointmentRepository appointmentRepository) : IRequestHandler<IsBlockingAppointmentRequest, bool>
{
    public async Task<bool> Handle(IsBlockingAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, request.Date, request.Date, request.EmployeeId, null);
        return appointments.Exists(a => a.Id != request.AppointmentId && request.Start < a.TimeEnd && request.End > a.TimeStart);
    }
}