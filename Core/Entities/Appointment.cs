namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>An appointment of a customer with an employee.</summary>
public class Appointment
{
    /// <summary>Gets or sets the id of the company.</summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>Gets or sets the id of the customer.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the date of the appointment.</summary>
    public DateOnly Date { get; set; }

    /// <summary>Gets or sets the employee associated with the appointment.</summary>
    public Employee Employee { get; set; } = null!;

    /// <summary>Gets or sets the id of the employee associated with the appointment.</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>Gets or sets the employee that can replace the original employee.</summary>
    public Employee? EmployeeReplacement { get; set; }

    /// <summary>Gets or sets the id of the employee that can replace the original employee.</summary>
    public Guid? EmployeeReplacementId { get; set; }

    /// <summary>Gets or sets the id of the appointment.</summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment is done.</summary>
    public bool IsDone { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment is signed by the customer.</summary>
    public bool IsSignedByCustomer { get; set; }

    /// <summary>Gets or sets a value indicating whether the appointment is signed by the employee.</summary>
    public bool IsSignedByEmployee { get; set; }

    /// <summary>Gets or sets the notes.</summary>
    public string Notes { get; set; } = "";

    /// <summary>Gets or sets the starting time.</summary>
    public TimeOnly TimeEnd { get; set; }

    /// <summary>Gets or sets the ending time.</summary>
    public TimeOnly TimeStart { get; set; }
}