namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

/// <summary>Repository for <see cref="Appointment" />.</summary>
public interface IAppointmentRepository
{
    /// <summary>Adds an appointment.</summary>
    /// <param name="appointment">The appointment to add.</param>
    /// <returns>The added appointment.</returns>
    Task<Appointment> AddAppointmentAsync(Appointment appointment);

    /// <summary>Deletes an appointment.</summary>
    /// <param name="appointment">The appointment to delete.</param>
    /// <returns>An awaitable task.</returns>
    Task DeleteAppointmentAsync(Appointment appointment);

    /// <summary>Gets an appointment by id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="appointmentId">The appointment id.</param>
    /// <returns>An appointment or null if none is found.</returns>
    Task<Appointment?> GetAppointmentAsync(Guid companyId, Guid appointmentId);

    /// <summary>Gets appointments matching the provided filters.</summary>
    /// <param name="companyId">The id of the company.</param>
    /// <param name="from">The start date of the appointments.</param>
    /// <param name="to">The end date of the appointments.</param>
    /// <param name="employeeId">The id of the employee associated with appointments.</param>
    /// <param name="customerId">The id of the appointment's customer.</param>
    Task<List<Appointment>> GetAppointmentsAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId);

    /// <summary>Gets all planned appointments of the current month.</summary>
    /// <returns>A list of appointments.</returns>
    Task<List<Appointment>> GetPlannedAppointmentsOfCurrentMonthAsync();

    /// <summary>Updates an appointment.</summary>
    /// <param name="appointment">The modified appointment.</param>
    /// <returns>The updated appointment.</returns>
    Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
}