namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Enums;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursListRequest(
    User user,
    int year,
    int month) : IRequest<List<GetWorkingTimeReportListDto>>
{
    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class WorkingHoursListRequestHandler(IWorkingTimeRepository workingTimeRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<WorkingHoursListRequest, List<GetWorkingTimeReportListDto>>
{
    public async Task<List<GetWorkingTimeReportListDto>> Handle(WorkingHoursListRequest request, CancellationToken cancellationToken)
    {
        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var workedMonths = await workingTimeRepository.GetWorkedMonthsAsync(
            request.User.CompanyId,
            start,
            end,
            request.User.IsManager ? null : request.User.EmployeeId);

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

        return workedMonths.OrderBy(o => o.EmployeeName).ToList();
    }
}