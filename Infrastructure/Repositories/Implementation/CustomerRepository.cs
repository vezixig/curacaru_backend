namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

#pragma warning disable S6603 trueForAll is not convertible to sql
[Repository]
internal class CustomerRepository(DataContext dataContext) : ICustomerRepository
{
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) dataContext.Attach(customer.AssociatedEmployee!);
        if (customer.InsuranceId.HasValue) dataContext.Attach(customer.Insurance!);
        if (customer.ZipCity != null) dataContext.Attach(customer.ZipCity);

        var result = await dataContext.Customers.AddAsync(customer);
        await dataContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        dataContext.Customers.Remove(customer);
        await dataContext.SaveChangesAsync();
    }

    public Task<List<Customer>> GetAllCustomersAsync()
        => dataContext.Customers.Where(o => o.Status == CustomerStatus.Customer).ToListAsync();

    public Task<Customer?> GetCustomerAsync(
        Guid companyId,
        Guid customerId,
        Guid? employeeId = null,
        bool asTracking = false)
    {
        var query = dataContext.Customers.AsQueryable();
        if (asTracking) query = query.AsTracking();

        return query.Include(o => o.AssociatedEmployee)
            .Include(o => o.Insurance)
            .ThenInclude(o => o!.ZipCity)
            .Include(o => o.Products)
            .Include(o => o.ZipCity)
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == customerId && (!employeeId.HasValue || o.AssociatedEmployeeId == employeeId));
    }

    public Task<int> GetCustomerCountAsync(Guid companyId, Guid? employeeId, CustomerStatus status)
    {
        var result = dataContext.Customers.Where(o => o.CompanyId == companyId && o.Status == status);
        if (employeeId.HasValue) result = result.Where(o => o.AssociatedEmployeeId == employeeId.Value);
        return result.CountAsync();
    }

    public Task<List<Customer>> GetCustomersAsync(
        Guid companyId,
        Guid? employeeId = null,
        InsuranceStatus? insuranceStatus = null,
        int? requestAssignmentDeclarationYear = null,
        Guid? customerId = null,
        CustomerStatus? status = null,
        int? page = null,
        int? pageSize = null)
    {
        var result = dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId);

        if (employeeId.HasValue) result = result.Where(c => c.AssociatedEmployeeId == employeeId.Value);

        if (customerId.HasValue) result = result.Where(c => c.Id == customerId);

        if (status.HasValue) result = result.Where(c => c.Status == null || c.Status == status);

        if (insuranceStatus.HasValue) result = result.Where(c => c.InsuranceStatus == insuranceStatus.Value);

        if (requestAssignmentDeclarationYear.HasValue)
            result = result.Include(o => o.AssignmentDeclarations).Where(c => c.AssignmentDeclarations.All(a => a.Year != requestAssignmentDeclarationYear));

        result = result.OrderBy(c => c.LastName);

        if (page.HasValue && pageSize.HasValue) result = result.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return result
            .OrderBy(c => c.LastName)
            .ToListAsync();
    }

    public Task<List<Customer>> GetCustomersForDeploymentReportsAsync(
        Guid companyId,
        Guid? employeeId)
    {
        var result = dataContext.Customers.Where(o => o.CompanyId == companyId && o.Status == CustomerStatus.Customer);

        if (employeeId.HasValue)
            result = result.Where(
                o => o.AssociatedEmployeeId == employeeId
                     || o.Appointments.Any(
                         appointment => appointment.EmployeeId == employeeId || appointment.EmployeeReplacementId == employeeId));

        return result.ToListAsync();
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        //   if (customer.AssociatedEmployeeId.HasValue) dataContext.Attach(customer.AssociatedEmployee!);
        //    if (customer.InsuranceId.HasValue) dataContext.Attach(customer.Insurance!);
        // if (customer.ZipCity != null) dataContext.Attach(customer.ZipCity);

        var result = dataContext.Customers.Update(customer);
        await dataContext.SaveChangesAsync();
        return result.Entity;
    }
}
#pragma warning restore S6603