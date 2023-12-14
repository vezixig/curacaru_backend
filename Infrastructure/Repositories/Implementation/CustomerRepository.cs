﻿namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

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

    public Task<Customer?> GetCustomerAsync(Guid requestCompanyId, Guid requestEmployeeId)
        => _dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .FirstOrDefaultAsync(o => o.CompanyId == requestCompanyId && o.Id == requestEmployeeId);

    public Task<List<Customer>> GetCustomersAsync(Guid companyId)
        => _dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId)
            .ToListAsync();

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) _dataContext.Attach(customer.AssociatedEmployee);
        _dataContext.Attach(customer.ZipCity);

        var result = _dataContext.Customers.Update(customer);
        return _dataContext.SaveChangesAsync().ContinueWith(_ => result.Entity);
    }
}