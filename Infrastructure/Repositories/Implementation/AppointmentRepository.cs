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
            .Where(a => a.CompanyId == companyId);

        if (from.HasValue) query = query.Where(a => a.Date >= from.Value);
        if (to.HasValue) query = query.Where(a => a.Date <= to.Value);
        if (employeeId.HasValue) query = query.Where(a => a.EmployeeId == employeeId.Value || a.EmployeeReplacementId == employeeId.Value);
        if (customerId.HasValue) query = query.Where(a => a.CustomerId == customerId.Value);

        return query.ToListAsync();
    }
}