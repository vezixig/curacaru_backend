namespace Curacaru.Backend.Core.DTO.TimeTracker;

using System.ComponentModel.DataAnnotations;

public class GetWorkingTimeReportDto
{
    public Guid CompanyId { get; set; }

    public Guid EmployeeId { get; set; }

    [Key]
    public Guid Id { get; set; }

    public int Month { get; set; }

    public string SignatureEmployee { get; set; } = "";

    public string SignatureEmployeeCity { get; set; } = "";

    public DateOnly SignatureEmployeeDate { get; set; }

    public double TotalHours { get; set; }

    public int Year { get; set; }
}