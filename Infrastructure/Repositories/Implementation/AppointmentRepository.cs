namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class AppointmentRepository(DataContext dataContext) : IAppointmentRepository
{
    public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
    {
        dataContext.Attach(appointment.Employee);
        if (appointment.EmployeeReplacement != null) dataContext.Attach(appointment.EmployeeReplacement);
        dataContext.Attach(appointment.Customer);

        var dbAppointment = dataContext.Appointments.Add(appointment);
        await dataContext.SaveChangesAsync();
        return dbAppointment.Entity;
    }

    public Task DeleteAppointmentAsync(Appointment appointment)
    {
        dataContext.Appointments.Remove(appointment);
        return dataContext.SaveChangesAsync();
    }

    public Task<Appointment?> GetAppointmentAsync(Guid companyId, Guid appointmentId)
        => dataContext.Appointments.Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == appointmentId && o.CompanyId == companyId);

    public Task<List<Appointment>> GetAppointmentsAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId)

    {
        var query = dataContext.Appointments
            .Include(o => o.Customer)
            .ThenInclude(o => o.ZipCity)
            .Include(o => o.Employee)
            .Include(o => o.EmployeeReplacement)
            .Where(o => o.CompanyId == companyId);

        if (from.HasValue) query = query.Where(o => o.Date >= from.Value);
        if (to.HasValue) query = query.Where(o => o.Date <= to.Value);
        if (employeeId.HasValue) query = query.Where(o => o.EmployeeId == employeeId.Value || o.EmployeeReplacementId == employeeId.Value);
        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId.Value);

        query = query.OrderBy(o => o.Date).ThenBy(o => o.TimeStart);

        return query.ToListAsync();
    }

    public Task<List<Appointment>> GetPlannedAppointmentsOfCurrentMonthAsync()
        => dataContext.Appointments
            .Where(
                o => o.IsPlanned
                     && o.Date > new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1)
                     && o.Date < new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1))
            .ToListAsync();

    public Task<bool> IsAppointmentReplacement(Guid customerId, Guid employeeId)
        => dataContext.Appointments.AnyAsync(o => o.CustomerId == customerId && o.EmployeeReplacementId != employeeId);

    public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
    {
        if (appointment.Employee is not null) dataContext.Attach(appointment.Employee);
        if (appointment.Customer is not null) dataContext.Attach(appointment.Customer);
        if (appointment.EmployeeReplacement is not null) dataContext.Attach(appointment.EmployeeReplacement);

        var dbAppointment = dataContext.Appointments.Update(appointment);
        await dataContext.SaveChangesAsync();
        return dbAppointment.Entity;
    }
}