namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO;
using Core.DTO.TimeTracker;
using Core.Enums;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursListRequest(
    User user,
    int year,
    int month,
    int page,
    int pageSize) : IRequest<PageDto<GetWorkingTimeReportListDto>>
{
    public int Month { get; } = month;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class WorkingHoursListRequestHandler(IWorkingTimeRepository workingTimeRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<WorkingHoursListRequest, PageDto<GetWorkingTimeReportListDto>>
{
    public async Task<PageDto<GetWorkingTimeReportListDto>> Handle(WorkingHoursListRequest request, CancellationToken cancellationToken)
    {
        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var workedMonthsCount = await workingTimeRepository.GetWorkedMonthsCountAsync(
            request.User.CompanyId,
            start,
            end,
            request.User.IsManager ? null : request.User.EmployeeId);
        var workedMonths = await workingTimeRepository.GetWorkedMonthsAsync(
            request.User.CompanyId,
            start,
            end,
            request.User.IsManager ? null : request.User.EmployeeId,
            request.Page,
            request.PageSize);

        var reports = await workingTimeRepository.GetWorkingTimeReportsAsync(
            request.User.CompanyId,
            request.Year,
            request.Month,
            request.User.IsManager ? null : request.User.EmployeeId);

        var employees = await employeeRepository.GetEmployeesAsync(request.User.CompanyId);

        workedMonths.ForEach(
            workedMonth =>
            {
                var report = reports.Find(report => report.EmployeeId == workedMonth.EmployeeId);
                workedMonth.Status = report switch
                {
                    null => WorkingHoursReportStatus.NotSigned,
                    { SignatureManagerDate: null } => WorkingHoursReportStatus.EmployeeSigned,
                    _ => WorkingHoursReportStatus.ManagerSigned
                };
                workedMonth.EmployeeName = employees.Find(employee => employee.Id == workedMonth.EmployeeId)!.FullName;
                workedMonth.Id = report?.Id;
            });

        return new(workedMonths, request.Page, (int)Math.Ceiling((double)workedMonthsCount / request.PageSize));
    }
}