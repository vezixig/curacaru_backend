namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using AutoMapper;
using Core.DTO.TimeTracker;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursReportRequest(
    User user,
    Guid employeeId,
    int month,
    int year) : IRequest<GetWorkingTimeReportDto?>
{
    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class WorkingHoursReportRequestHandler(IWorkingTimeRepository workingTimeRepository, IMapper mapper)
    : IRequestHandler<WorkingHoursReportRequest, GetWorkingTimeReportDto?>
{
    public async Task<GetWorkingTimeReportDto?> Handle(WorkingHoursReportRequest request, CancellationToken cancellationToken)
    {
        if (!request.User.IsManager && request.User.EmployeeId != request.EmployeeId)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten abrufen");

        var reports = await workingTimeRepository.GetWorkingTimeReportsAsync(request.User.CompanyId, request.Year, request.Month, request.EmployeeId);
        var report = reports.FirstOrDefault();

        return mapper.Map<GetWorkingTimeReportDto?>(report);
    }
}