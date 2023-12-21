namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class AppointmentRepository(DataContext dataContext) : IAppointmentRepository
{
    public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
    {
        dataContext.Attach(appointment.Employee);
        dataContext.Attach(appointment.Customer);
        if (appointment.EmployeeReplacement != null) dataContext.Attach(appointment.EmployeeReplacement);

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
        => dataContext.Appointments.FirstOrDefaultAsync(o => o.Id == appointmentId && o.CompanyId == companyId);

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

        return query.ToListAsync();
    }

    public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
    {
        dataContext.Attach(appointment.Employee);
        dataContext.Attach(appointment.Customer);
        if (appointment.EmployeeReplacement != null) dataContext.Attach(appointment.EmployeeReplacement);

        var dbAppointment = dataContext.Appointments.Update(appointment);
        await dataContext.SaveChangesAsync();
        return dbAppointment.Entity;
    }
}