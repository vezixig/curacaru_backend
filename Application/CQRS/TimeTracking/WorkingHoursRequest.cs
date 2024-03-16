namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursRequest(
    Guid companyId,
    Guid employeeId,
    string authId,
    int month,
    int year) : IRequest<List<GetWorkedHoursDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class WorkingHoursRequestHandler(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<WorkingHoursRequest, List<GetWorkedHoursDto>>
{
    public async Task<List<GetWorkedHoursDto>> Handle(WorkingHoursRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (!user!.IsManager && user!.Id != request.EmployeeId) throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten abrufen");

        var workingHours = await appointmentRepository.GetAppointmentsAsync(
            request.CompanyId,
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
                    IsDone = wh.IsDone
                })
            .OrderBy(o => o.Date)
            .ThenBy(o => o.TimeStart)
            .ToList();
    }
}