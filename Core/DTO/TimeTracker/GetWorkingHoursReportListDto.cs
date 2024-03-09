namespace Curacaru.Backend.Core.DTO.TimeTracker;

using Enums;

public class GetWorkingHoursReportListDto
{
    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public Guid? Id { get; set; }

    public int Month { get; set; }

    public WorkingHoursReportStatus Status { get; set; }

    public double WorkedHours { get; set; }

    public int Year { get; set; }
}