namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Enums;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursListRequest(
    Guid companyId,
    string authId,
    int year,
    int month) : IRequest<List<GetWorkingTimeReportListDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class WorkingHoursListRequestHandler(IWorkingTimeRepository workingTimeRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<WorkingHoursListRequest, List<GetWorkingTimeReportListDto>>
{
    public async Task<List<GetWorkingTimeReportListDto>> Handle(WorkingHoursListRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var workedMonths = await workingTimeRepository.GetWorkedMonthsAsync(
            request.CompanyId,
            start,
            end,
            user!.IsManager ? null : user.Id);

        var reports = await workingTimeRepository.GetWorkingTimeReportsAsync(request.CompanyId, request.Year, request.Month, user!.IsManager ? null : user.Id);

        var employees = await employeeRepository.GetEmployeesAsync(request.CompanyId);

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
            });

        return workedMonths;
    }
}