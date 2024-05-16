namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;
using Core.Enums;
using Core.Models;

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

    /// <summary>Deletes a list of appointments.</summary>
    /// <param name="appointments">The appointments to delete.</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteAppointmentsAsync(List<Appointment> appointments);

    /// <summary>Gets an appointment by id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="appointmentId">The appointment id.</param>
    /// <returns>An appointment or null if none is found.</returns>
    Task<Appointment?> GetAppointmentAsync(Guid companyId, Guid appointmentId);

    /// <summary>Gets the count of appointments for the provided filters.</summary>
    /// <param name="companyId">The id of the company.</param>
    /// <param name="from">The start date of the appointments.</param>
    /// <param name="to">The end date of the appointments.</param>
    /// <param name="employeeId">The id of the employee associated with appointments.</param>
    /// <param name="customerId">The id of the appointment's customer.</param>
    /// <param name="onlyOpen">An optional filter to only count open appointments.</param>
    /// <param name="clearanceType">An optional filter for the clearance type.</param>
    Task<int> GetAppointmentCountAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId,
        bool? onlyOpen = null,
        ClearanceType? clearanceType = null);

    /// <summary>Gets appointments matching the provided filters.</summary>
    /// <param name="companyId">The id of the company.</param>
    /// <param name="from">The start date of the appointments.</param>
    /// <param name="to">The end date of the appointments.</param>
    /// <param name="employeeId">The id of the employee associated with appointments.</param>
    /// <param name="customerId">The id of the appointment's customer.</param>
    /// <param name="onlyOpen">An optional filter to only return open appointments.</param>
    /// <param name="clearanceType">An optional filter for the clearance type.</param>
    /// <param name="asTracking">Indicates whether to track the appointments.</param>
    Task<List<Appointment>> GetAppointmentsAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId,
        bool? onlyOpen = null,
        int? page = null,
        int? pageSize = null,
        ClearanceType? clearanceType = null,
        bool asTracking = false);

    /// <summary>Gets the clearance types of the appointments of the given month and year grouped by employee.</summary>
    /// <param name="companyId">The id of the company.</param>
    /// <param name="customerId">An optional customer id.</param>
    /// <param name="employeeId">An optional employee id.</param>
    /// <param name="year">The year to filter by.</param>
    /// <param name="month">The month to filter by.</param>
    /// <returns></returns>
    Task<List<AppointmentClearance>> GetClearanceTypes(
        Guid companyId,
        Guid? customerId,
        Guid? employeeId,
        int year,
        int month,
        int page,
        int pageSize);

    Task<int> GetClearanceTypesCount(
        Guid userCompanyId,
        Guid? requestCustomerId,
        Guid? requestEmployeeId,
        int requestYear,
        int requestMonth);

    /// <summary>Gets all planned appointments of the current month.</summary>
    /// <returns>A list of appointments.</returns>
    Task<List<Appointment>> GetPlannedAppointmentsOfCurrentMonthAsync();

    /// <summary>Checks if an employee is a replacement for an appointment.</summary>
    /// <param name="customerId">The id of the customer.</param>
    /// <param name="employeeId">The id of the employee.</param>
    /// <returns>True if the employee is a replacement for the customer's appointment, otherwise false.</returns>
    Task<bool> IsAppointmentReplacement(Guid customerId, Guid employeeId);

    /// <summary>Updates an appointment.</summary>
    /// <param name="appointment">The modified appointment.</param>
    /// <returns>The updated appointment.</returns>
    Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
}