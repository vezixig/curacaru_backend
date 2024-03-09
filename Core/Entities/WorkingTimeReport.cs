namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

public class WorkingTimeReport
{
    public Guid CompanyId { get; set; }

    public Employee Employee { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    [Key]
    public Guid Id { get; set; }

    public Employee? Manager { get; set; }

    public Guid? ManagerId { get; set; }

    public int Month { get; set; }

    public string SignatureEmployee { get; set; } = "";

    public string SignatureEmployeeCity { get; set; } = "";

    public DateOnly SignatureEmployeeDate { get; set; }

    public string? SignatureManager { get; set; } = "";

    public string? SignatureManagerCity { get; set; } = "";

    public DateOnly? SignatureManagerDate { get; set; }

    public double TotalHours { get; set; }

    public int Year { get; set; }
}