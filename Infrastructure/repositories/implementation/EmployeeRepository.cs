namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly DataContext _dataContext;

    public EmployeeRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        var dbEmployee = _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }

    public Task<Employee?> GetEmployeeByAuthIdAsync(string authId)
        => _dataContext.Employees.FirstOrDefaultAsync(e => e.AuthId == authId);

    public Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, Guid companyId)
        => _dataContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId && e.CompanyId == companyId);

    public Task<List<Employee>> GetEmployeesAsync(Guid companyId)
        => _dataContext.Employees.Where(e => e.CompanyId == companyId).ToListAsync();

    public async Task<Employee> UpdateEmployeeAsync(Employee currentEmploye)
    {
        var dbEmployee = _dataContext.Employees.Update(currentEmploye);
        await _dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }
}