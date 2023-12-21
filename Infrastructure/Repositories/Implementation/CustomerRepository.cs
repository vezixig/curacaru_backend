namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class CustomerRepository(DataContext dataContext) : ICustomerRepository
{
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) dataContext.Attach(customer.AssociatedEmployee);
        if (customer.InsuranceId.HasValue) dataContext.Attach(customer.Insurance);
        dataContext.Attach(customer.ZipCity);

        var result = await dataContext.Customers.AddAsync(customer);
        await dataContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        dataContext.Customers.Remove(customer);
        await dataContext.SaveChangesAsync();
    }

    public Task<Customer?> GetCustomerAsync(Guid companyId, Guid employeeId)
        => dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Include(o => o.Insurance)
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == employeeId);

    public Task<List<Customer>> GetCustomersAsync(Guid companyId, Guid? employeeId = null)
    {
        var result = dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId);

        if (employeeId.HasValue) result = result.Where(c => c.AssociatedEmployeeId == employeeId.Value);

        return result
            .OrderBy(c => c.LastName)
            .ToListAsync();
    }

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) dataContext.Attach(customer.AssociatedEmployee);
        if (customer.InsuranceId.HasValue) dataContext.Attach(customer.Insurance);
        dataContext.Attach(customer.ZipCity);

        var result = dataContext.Customers.Update(customer);
        return dataContext.SaveChangesAsync().ContinueWith(_ => result.Entity);
    }
}