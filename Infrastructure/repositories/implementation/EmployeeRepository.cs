namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly DataContext _dataContext;

    public EmployeeRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public async Task<Employee> AddEmployee(Employee employee)
    {
        var dbEmployee = _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();
        return dbEmployee.Entity;
    }

    public Task<Employee?> GetEmployeeByAuthId(string authId)
        => _dataContext.Employees.FirstOrDefaultAsync(e => e.AuthId == authId);
}