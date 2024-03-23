namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

#pragma warning disable S6603 - trueForAll is not convertible to sql
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
        => dataContext.Customers.ToListAsync();

    public Task<Customer?> GetCustomerAsync(Guid companyId, Guid customerId, Guid? employeeId = null)
        => dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Include(o => o.Insurance)
            .ThenInclude(o => o!.ZipCity)
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == customerId && (!employeeId.HasValue || o.AssociatedEmployeeId == employeeId));

    public Task<List<Customer>> GetCustomersAsync(
        Guid companyId,
        Guid? employeeId = null,
        InsuranceStatus? insuranceStatus = null,
        int? requestAssignmentDeclarationYear = null)
    {
        var result = dataContext.Customers
            .Include(o => o.AssociatedEmployee)
            .Include(o => o.ZipCity)
            .Where(c => c.CompanyId == companyId);

        if (employeeId.HasValue) result = result.Where(c => c.AssociatedEmployeeId == employeeId.Value);

        if (insuranceStatus.HasValue) result = result.Where(c => c.InsuranceStatus == insuranceStatus.Value);

        if (requestAssignmentDeclarationYear.HasValue)
            result = result.Include(o => o.AssignmentDeclarations).Where(c => c.AssignmentDeclarations.All(a => a.Year != requestAssignmentDeclarationYear));

        return result
            .OrderBy(c => c.LastName)
            .ToListAsync();
    }

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        if (customer.AssociatedEmployeeId.HasValue) dataContext.Attach(customer.AssociatedEmployee!);
        if (customer.InsuranceId.HasValue) dataContext.Attach(customer.Insurance!);
        if (customer.ZipCity != null) dataContext.Attach(customer.ZipCity);

        var result = dataContext.Customers.Update(customer);
        return dataContext.SaveChangesAsync().ContinueWith(_ => result.Entity);
    }
}
#pragma warning restore S6603