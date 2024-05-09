namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;

[Repository]
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

    public Task<int> GetAppointmentCountAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId,
        ClearanceType? clearanceType = null)
    {
        var query = dataContext.Appointments.Where(o => o.CompanyId == companyId);

        if (from.HasValue) query = query.Where(o => o.Date >= from.Value);
        if (to.HasValue) query = query.Where(o => o.Date <= to.Value);
        if (employeeId.HasValue) query = query.Where(o => o.EmployeeId == employeeId.Value || o.EmployeeReplacementId == employeeId.Value);
        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId.Value);
        if (clearanceType.HasValue) query = query.Where(o => o.ClearanceType == clearanceType.Value);

        return query.CountAsync();
    }

    public Task<List<Appointment>> GetAppointmentsAsync(
        Guid companyId,
        DateOnly? from,
        DateOnly? to,
        Guid? employeeId,
        Guid? customerId,
        int? page = null,
        int? pageSize = null,
        ClearanceType? clearanceType = null,
        bool asTracking = false)

    {
        var query = dataContext.Appointments
            .Include(o => o.Customer)
            .ThenInclude(o => o.ZipCity)
            .Include(o => o.Employee)
            .Include(o => o.EmployeeReplacement)
            .Where(o => o.CompanyId == companyId);

        if (asTracking) query = query.AsTracking();

        // optional filters
        if (from.HasValue) query = query.Where(o => o.Date >= from.Value);
        if (to.HasValue) query = query.Where(o => o.Date <= to.Value);
        if (employeeId.HasValue) query = query.Where(o => o.EmployeeId == employeeId.Value || o.EmployeeReplacementId == employeeId.Value);
        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId.Value);
        if (clearanceType.HasValue) query = query.Where(o => o.ClearanceType == clearanceType.Value);

        query = query.OrderBy(o => o.Date).ThenBy(o => o.TimeStart);

        // paging
        if (page.HasValue && pageSize.HasValue) query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return query.ToListAsync();
    }

    public Task<List<AppointmentClearance>> GetClearanceTypes(
        Guid companyId,
        Guid? customerId,
        Guid? employeeId,
        int year,
        int month)
    {
        var query = dataContext.Appointments.Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.EmployeeReplacement)
            .Where(o => o.CompanyId == companyId && o.Date.Year == year && o.Date.Month == month && o.ClearanceType != null);

        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId.Value);
        if (employeeId.HasValue) query = query.Where(o => o.EmployeeId == employeeId.Value || o.EmployeeReplacementId == employeeId.Value);

        return query.GroupBy(o => new { o.CustomerId, o.ClearanceType })
            .Select(
                o => new AppointmentClearance
                {
                    Customer = o.First().Customer,
                    Employees = o.Select(p => p.Employee).ToList(),
                    ReplacementEmployee = o.Where(p => p.EmployeeReplacement != null).Select(p => p.EmployeeReplacement!).ToList(),
                    ClearanceType = o.Key.ClearanceType!.Value
                })
            .ToListAsync();
    }

    public Task<List<Appointment>> GetPlannedAppointmentsOfCurrentMonthAsync()
        => dataContext.Appointments
            .Where(
                o => o.IsPlanned
                     && o.Date >= new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1)
                     && o.Date <= new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1))
            .OrderBy(o => o.Date)
            .ThenBy(o => o.TimeStart)
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