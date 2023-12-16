namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _dataContext;

    public CustomerRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) _dataContext.Attach(customer.AssociatedEmployee);
        if (customer.InsuranceId.HasValue) _dataContext.Attach(customer.Insurance);
        _dataContext.Attach(customer.ZipCity);

        var result = await _dataContext.Customers.AddAsync(customer);
        await _dataContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        _dataContext.Customers.Remove(customer);
        await _dataContext.SaveChangesAsync();
    }

    public Task<Customer?> GetCustomerAsync(Guid companyId, Guid employeeId)
        => _dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Include(o => o.Insurance)
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == employeeId);

    public Task<List<Customer>> GetCustomersAsync(Guid companyId)
        => _dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId)
            .OrderBy(c => c.LastName)
            .ToListAsync();

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) _dataContext.Attach(customer.AssociatedEmployee);
        if (customer.InsuranceId.HasValue) _dataContext.Attach(customer.Insurance);
        _dataContext.Attach(customer.ZipCity);

        var result = _dataContext.Customers.Update(customer);
        return _dataContext.SaveChangesAsync().ContinueWith(_ => result.Entity);
    }
}