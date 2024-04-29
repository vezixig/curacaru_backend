namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

/// <summary>An appointment of a customer with an employee.</summary>
public class Appointment
{
    /// <summary>Gets or sets the type of the clearance.</summary>
    public ClearanceType ClearanceType { get; set; }

    /// <summary>Gets or sets the id of the company.</summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the costs of the appointment subtracted from the current budget.</summary>
    public decimal Costs { get; set; }

    /// <summary>Gets or sets the costs of the appointment subtracted from last year's budget.</summary>
    public decimal CostsLastYearBudget { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>Gets or sets the id of the customer.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the date of the appointment.</summary>
    public DateOnly Date { get; set; }

    /// <summary>Gets or sets the id of the deployment report.</summary>
    public Guid? DeploymentReportId { get; set; }

    /// <summary>Gets or sets the distance in km to the customer.</summary>
    public int DistanceToCustomer { get; set; }

    /// <summary>Gets or sets the employee associated with the appointment.</summary>
    public Employee Employee { get; set; } = null!;

    /// <summary>Gets or sets the id of the employee associated with the appointment.</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>Gets or sets the employee that can replace the original employee.</summary>
    public Employee? EmployeeReplacement { get; set; }

    /// <summary>Gets or sets the id of the employee that can replace the original employee.</summary>
    public Guid? EmployeeReplacementId { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment has a budget error.</summary>
    /// <remarks>This is set by the budget replenisher if the budget the appointment is set to, is used up.</remarks>
    public bool HasBudgetError { get; set; }

    /// <summary>Gets or sets the id of the appointment.</summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment is done.</summary>
    public bool IsDone { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment is planned after the current month.</summary>
    public bool IsPlanned { get; set; }

    /// <summary>Gets or sets the notes.</summary>
    [MaxLength(500)]
    public string Notes { get; set; } = "";

    /// <summary>Gets or sets the signature of the employee.</summary>
    [MaxLength(15000)]
    public string SignatureCustomer { get; set; } = "";

    /// <summary>Gets or sets the signature of the customer.</summary>
    [MaxLength(15000)]
    public string SignatureEmployee { get; set; } = "";

    /// <summary>Gets or sets the starting time.</summary>
    public TimeOnly TimeEnd { get; set; }

    /// <summary>Gets or sets the ending time.</summary>
    public TimeOnly TimeStart { get; set; }
}