namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

/// <summary>Repository for <see cref="Appointment" />.</summary>
public interface IAppointmentRepository
{
    /// <summary>Gets appointments matching the provided filters.</summary>
    /// <param name="companyId">The id of the company.</param>
    /// <param name="from">The start date of the appointments.</param>
    /// <param name="to">The end date of the appointments.</param>
    /// <param name="employeeId">The id of the employee associated with appointments.</param>
    /// <param name="customerId">The id of the appointment's customer.</param>
    public Task<List<Appointment>> GetAppointmentsAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId);
}