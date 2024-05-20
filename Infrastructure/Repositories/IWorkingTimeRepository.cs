namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.DTO.TimeTracker;
using Core.Entities;

public interface IWorkingTimeRepository
{
    /// <summary>Adds a new working time report.</summary>
    /// <param name="report">The new report.</param>
    /// <returns>An awaitable task object.</returns>
    Task AddWorkingTimeReportAsync(WorkingTimeReport report);

    /// <summary>Deletes a working time report.</summary>
    /// <param name="report">The report to delete.</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteWorkingTimeReportAsync(WorkingTimeReport report);

    Task<List<GetWorkingTimeReportListDto>> GetWorkedMonthsAsync(
        Guid requestCompanyId,
        DateOnly start,
        DateOnly end,
        Guid? userId,
        int page,
        int pageSize);

    /// <summary>Gets the count of working employees in the month.</summary>
    Task<int> GetWorkedMonthsCountAsync(
        Guid requestCompanyId,
        DateOnly start,
        DateOnly end,
        Guid? userId);

    /// <summary>Gets a working time report by its id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="reportId">The report id.</param>
    Task<WorkingTimeReport?> GetWorkingTimeReportByIdAsync(Guid companyId, Guid reportId);

    Task<List<WorkingTimeReport>> GetWorkingTimeReportsAsync(
        Guid requestCompanyId,
        int year,
        int month,
        Guid? userId);

    Task UpdateWorkingTimeReportAsync(WorkingTimeReport report);
}