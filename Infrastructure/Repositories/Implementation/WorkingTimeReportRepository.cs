namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.DTO.TimeTracker;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class WorkingTimeReportRepository(DataContext dataContext) : IWorkingTimeRepository
{
    public Task AddWorkingTimeReportAsync(WorkingTimeReport report)
    {
        dataContext.WorkingTimeReports.Add(report);
        return dataContext.SaveChangesAsync();
    }

    public Task<List<GetWorkingTimeReportListDto>> GetWorkedMonthsAsync(
        Guid requestCompanyId,
        DateOnly start,
        DateOnly end,
        Guid? userId)
        => dataContext.Appointments
            .Where(
                o => o.CompanyId == requestCompanyId && o.Date >= start && o.Date <= end && (userId == null || (o.EmployeeReplacementId ?? o.EmployeeId) == userId))
            .GroupBy(o => new { EmployeeId = o.EmployeeReplacementId ?? o.EmployeeId, o.Date.Year, o.Date.Month })
            .Select(
                o => new GetWorkingTimeReportListDto
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
            .Include(o => o.Employee)
            .Where(
                o => o.CompanyId == requestCompanyId
                     && o.Year == year
                     && o.Month == month
                     && (userId == null || o.EmployeeId == (Guid)userId))
            .ToListAsync();

    public Task UpdateWorkingTimeReportAsync(WorkingTimeReport report)
    {
        dataContext.WorkingTimeReports.Update(report);
        return dataContext.SaveChangesAsync();
    }
}