namespace Curacaru.Backend.Core.DTO.Appointment;

public class UpdateAppointmentDto
{
    /// <inheritdoc cref="Entities.Appointment.CustomerId" />
    public Guid CustomerId { get; set; }

    /// <inheritdoc cref="Entities.Appointment.Date" />
    public DateOnly Date { get; set; }

    /// <inheritdoc cref="Entities.Appointment.EmployeeId" />
    public Guid EmployeeId { get; set; }

    /// <inheritdoc cref="Entities.Appointment.EmployeeReplacementId" />
    public Guid? EmployeeReplacementId { get; set; }

    /// <inheritdoc cref="Entities.Appointment.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Entities.Appointment.IsDone" />
    public bool IsDone { get; set; }

    /// <inheritdoc cref="Entities.Appointment.IsSignedByCustomer" />
    public bool IsSignedByCustomer { get; set; }

    /// <inheritdoc cref="Entities.Appointment.IsSignedByEmployee" />
    public bool IsSignedByEmployee { get; set; }

    /// <inheritdoc cref="Entities.Appointment.Notes" />
    public string Notes { get; set; } = "";

    /// <inheritdoc cref="Entities.Appointment.TimeEnd" />
    public TimeOnly TimeEnd { get; set; }

    /// <inheritdoc cref="Entities.Appointment.TimeStart" />
    public TimeOnly TimeStart { get; set; }
}