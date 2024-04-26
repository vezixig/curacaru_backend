namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursRequest(
    User user,
    Guid employeeId,
    int month,
    int year) : IRequest<List<GetWorkedHoursDto>>
{
    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class WorkingHoursRequestHandler(IAppointmentRepository appointmentRepository)
    : IRequestHandler<WorkingHoursRequest, List<GetWorkedHoursDto>>
{
    public async Task<List<GetWorkedHoursDto>> Handle(WorkingHoursRequest request, CancellationToken cancellationToken)
    {
        if (!request.User.IsManager && request.User.EmployeeId != request.EmployeeId)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten abrufen");

        var workingHours = await appointmentRepository.GetAppointmentsAsync(
            request.User.CompanyId,
            new DateOnly(request.Year, request.Month, 1),
            new DateOnly(request.Year, request.Month, 1).AddMonths(1).AddDays(-1),
            request.EmployeeId,
            null);

        return workingHours.Where(o => o.EmployeeReplacementId == request.EmployeeId || o.EmployeeReplacementId is null)
            .Select(
                wh => new GetWorkedHoursDto
                {
                    Date = wh.Date,
                    TimeEnd = wh.TimeEnd,
                    TimeStart = wh.TimeStart,
                    WorkDuration = (wh.TimeEnd - wh.TimeStart).TotalHours,
                    IsDone = wh.IsDone,
                    IsPlanned = wh.IsPlanned || wh.HasBudgetError,
                    AppointmentId = wh.Id
                })
            .OrderBy(o => o.Date)
            .ThenBy(o => o.TimeStart)
            .ToList();
    }
}