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

    [MaxLength(100000)]
    public string SignatureEmployee { get; set; } = "";

    [MaxLength(30)]
    public string SignatureEmployeeCity { get; set; } = "";

    public DateOnly SignatureEmployeeDate { get; set; }

    [MaxLength(100000)]
    public string? SignatureManager { get; set; } = "";

    [MaxLength(30)]
    public string? SignatureManagerCity { get; set; } = "";

    public DateOnly? SignatureManagerDate { get; set; }

    public double TotalHours { get; set; }

    public int Year { get; set; }
}