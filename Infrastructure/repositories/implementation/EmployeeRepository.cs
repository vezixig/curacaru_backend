namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly DataContext _dataContext;

    public EmployeeRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public Task<Employee?> GetEmployeeByAuthId(string authId)
        => _dataContext.Employees.FirstOrDefaultAsync(e => e.AuthId == authId);
}