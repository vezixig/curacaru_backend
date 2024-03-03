namespace Curacaru.Backend.Core.DTO.Appointment;

using Entities;

/// <summary>DTO for an appointment list entry.</summary>
/// <see cref="Appointment" />
public class GetAppointmentListEntryDto
{
    /// <summary>Gets or sets the city the appointment is taking place.</summary>
    public string City { get; set; } = "";

    /// <summary>Gets or sets the name of the customer.</summary>
    public string CustomerName { get; set; } = "";

    /// <inheritdoc cref="Appointment.Date" />
    public DateOnly Date { get; set; }

    /// <summary>Gets or sets the name of the employee.</summary>
    public string EmployeeName { get; set; } = "";

    /// <summary>Gets or sets the name of the employee replacement.</summary>
    public string? EmployeeReplacementName { get; set; }

    /// <inheritdoc cref="Appointment.HasBudgetError" />
    public bool HasBudgetError { get; set; }

    /// <inheritdoc cref="Appointment.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Appointment.IsDone" />
    public bool IsDone { get; set; }

    /// <inheritdoc cref="Appointment.IsPlanned" />
    public bool IsPlanned { get; set; }

    /// <inheritdoc cref="Customer.Phone" />
    public string Phone { get; set; } = "";

    /// <inheritdoc cref="Customer.Street" />
    public string Street { get; set; } = "";

    /// <inheritdoc cref="Appointment.TimeEnd" />
    public TimeOnly TimeEnd { get; set; }

    /// <inheritdoc cref="Appointment.TimeStart" />
    public TimeOnly TimeStart { get; set; }

    /// <inheritdoc cref="Customer.ZipCode" />
    public string ZipCode { get; set; } = "";
}