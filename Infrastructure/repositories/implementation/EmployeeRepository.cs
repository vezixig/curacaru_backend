namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class EmployeeRepository(DataContext dataContext) : IEmployeeRepository
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
        await dataContext.SaveChangesAsync();
    }

    public Task<bool> DoesEmailExistAsync(string email)
        => dataContext.Employees.AnyAsync(e => e.Email == email);

    public Task<Employee?> GetEmployeeByAuthIdAsync(string authId)
        => dataContext.Employees.FirstOrDefaultAsync(e => e.AuthId == authId);

    public Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId)
        => dataContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId && e.CompanyId == companyId);

    public Task<List<Employee>> GetEmployeesAsync(Guid companyId)
        => dataContext.Employees.Where(e => e.CompanyId == companyId).ToListAsync();

    public async Task<Employee> UpdateEmployeeAsync(Employee currentEmploye)
    {
        var dbEmployee = dataContext.Employees.Update(currentEmploye);
        await dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }
}