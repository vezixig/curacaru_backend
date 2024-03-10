namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using AutoMapper;
using Core.DTO.TimeTracker;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursReportRequest(
    Guid companyId,
    string authId,
    Guid employeeId,
    int month,
    int year) : IRequest<GetWorkingTimeReportDto?>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class WorkingHoursReportRequestHandler(IWorkingTimeRepository workingTimeRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<WorkingHoursReportRequest, GetWorkingTimeReportDto?>
{
    public async Task<GetWorkingTimeReportDto?> Handle(WorkingHoursReportRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (!user!.IsManager && user.Id != request.EmployeeId) throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten abrufen");

        var reports = await workingTimeRepository.GetWorkingTimeReportsAsync(request.CompanyId, request.Year, request.Month, request.EmployeeId);
        var report = reports.FirstOrDefault();

        return mapper.Map<GetWorkingTimeReportDto?>(report);
    }
}