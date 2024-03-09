namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.DTO.TimeTracker;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class WorkingHoursReportRepository(DataContext dataContext) : IWorkingHoursRepository
{
    public Task AddWorkingTimeReportAsync(WorkingTimeReport report)
    {
        dataContext.WorkingTimeReports.Add(report);
        return dataContext.SaveChangesAsync();
    }

    public Task<List<GetWorkingHoursReportListDto>> GetWorkedMonthsAsync(
        Guid requestCompanyId,
        DateOnly start,
        DateOnly end,
        Guid? userId)
        => dataContext.Appointments
            .Where(
                o => o.CompanyId == requestCompanyId && o.Date >= start && o.Date <= end && (userId == null || (o.EmployeeReplacementId ?? o.EmployeeId) == userId))
            .GroupBy(o => new { EmployeeId = o.EmployeeReplacementId ?? o.EmployeeId, o.Date.Year, o.Date.Month })
            .Select(
                o => new GetWorkingHoursReportListDto
                {
                    EmployeeId = o.First().EmployeeReplacementId ?? o.First().EmployeeId,
                    Year = o.Key.Year,
                    Month = o.Key.Month
                })
            .ToListAsync();

    public Task<List<WorkingTimeReport>> GetWorkingTimeReportsAsync(
        Guid requestCompanyId,
        int year,
        int month,
        Guid? userId)
        => dataContext.WorkingTimeReports
            .Where(
                o => o.CompanyId == requestCompanyId
                     && o.Year == year
                     && o.Month == month
                     && (userId == null || o.EmployeeId == (Guid)userId))
            .ToListAsync();
}