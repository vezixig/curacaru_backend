namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.DTO.TimeTracker;
using Core.Entities;

public interface IWorkingHoursRepository
{
    /// <summary>Adds a new working time report.</summary>
    /// <param name="report">The new report.</param>
    /// <returns>An awaitable task object.</returns>
    Task AddWorkingTimeReportAsync(WorkingTimeReport report);

    Task<List<GetWorkingHoursReportListDto>> GetWorkedMonthsAsync(
        Guid requestCompanyId,
        DateOnly start,
        DateOnly end,
        Guid? userId);

    Task<List<WorkingTimeReport>> GetWorkingTimeReportsAsync(
        Guid requestCompanyId,
        int year,
        int month,
        Guid? userId);

    Task UpdateWorkingTimeReportAsync(WorkingTimeReport report);
}