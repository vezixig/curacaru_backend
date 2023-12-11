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
        var result = await _dataContext.Customers.AddAsync(customer);
        await _dataContext.SaveChangesAsync();
        return result.Entity;
    }

    public Task<List<Customer>> GetCustomersAsync(Guid companyId)
        => _dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId)
            .ToListAsync();
}