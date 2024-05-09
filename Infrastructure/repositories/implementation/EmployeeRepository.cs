namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

[Repository]
internal class EmployeeRepository(DataContext dataContext, IMemoryCache memoryCache) : IEmployeeRepository
{
    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        var dbEmployee = dataContext.Employees.Add(employee);
        await dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }

    public async Task DeleteEmployeeAsync(Employee employee)
    {
        dataContext.Employees.Remove(employee);
        memoryCache.Remove($"{nameof(Employee)}{employee.AuthId}");
        await dataContext.SaveChangesAsync();
    }

    public Task<bool> DoesEmailExistAsync(string email)
        => dataContext.Employees.AnyAsync(e => e.Email == email);

    public async Task<Employee?> GetEmployeeByAuthIdAsync(string authId)
    {
        if (memoryCache.TryGetValue($"{nameof(Employee)}{authId}", out var cacheValue) && cacheValue is Employee employee) return employee;

        var dbEmployee = await dataContext.Employees.FirstOrDefaultAsync(e => e.AuthId == authId);
        if (dbEmployee != null) memoryCache.Set($"{nameof(Employee)}{authId}", dbEmployee);
        return dbEmployee;
    }

    public Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId)
        => dataContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId && e.CompanyId == companyId);

    public Task<List<Employee>> GetEmployeesAsync(Guid companyId, int? page = null, int? pageSize = null)
    {
        var result = dataContext.Employees.Where(e => e.CompanyId == companyId);
        if (page is not null && pageSize is not null) result = result.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return result.OrderBy(o => o.LastName).ThenBy(o => o.FirstName).ToListAsync();
    }

    public Task<int> GetEmployeesCountAsync(Guid companyId)
        => dataContext.Employees.CountAsync(e => e.CompanyId == companyId);

    public async Task<Employee> UpdateEmployeeAsync(Employee currentEmployee)
    {
        var dbEmployee = dataContext.Employees.Update(currentEmployee);
        memoryCache.Remove($"{nameof(Employee)}{currentEmployee.AuthId}");
        await dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }
}