namespace Curacaru.Backend.Core.DTO.Appointment;

using Entities;

/// <summary>A DTO to add an appointment.</summary>
/// <seealso cref="Appointment" />
public class AddAppointmentDto
{
    /// <inheritdoc cref="Appointment.CustomerId" />
    public Guid CustomerId { get; set; }

    /// <inheritdoc cref="Appointment.Date" />
    public DateOnly Date { get; set; }

    /// <inheritdoc cref="Appointment.DistanceToCustomer" />
    public int DistanceToCustomer { get; set; }

    /// <inheritdoc cref="Appointment.EmployeeId" />
    public Guid EmployeeId { get; set; }

    /// <inheritdoc cref="Appointment.EmployeeReplacementId" />
    public Guid? EmployeeReplacementId { get; set; }

    /// <inheritdoc cref="Appointment.IsSignedByCustomer" />
    public bool IsSignedByCustomer { get; set; }

    /// <inheritdoc cref="Appointment.IsSignedByEmployee" />
    public bool IsSignedByEmployee { get; set; }

    /// <inheritdoc cref="Appointment.Notes" />
    public string Notes { get; set; } = "";

    /// <inheritdoc cref="Appointment.TimeEnd" />
    public TimeOnly TimeEnd { get; set; }

    /// <inheritdoc cref="Appointment.TimeStart" />
    public TimeOnly TimeStart { get; set; }
}