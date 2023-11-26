namespace Curacaru.Backend.Infrastructure.repositories;

using Core.Entities;

public interface IEmployeeRepository
{
    /// <summary>Adds an employee to the database.</summary>
    public Task<Employee> AddEmployee(Employee employee);

    /// <summary>Checks if an employee with a given authId exists.</summary>
    /// <param name="authId">The auth id to check for.</param>
    /// <returns>True if an employee exists, otherwise false.</returns>
    public Task<Employee?> GetEmployeeByAuthId(string authId);

    /// <summary>Gets all employees of a company.</summary>
    /// <param name="companyId">The company id to get the employees for.</param>
    /// <returns>A list of employees.</returns>
    public Task<List<Employee>> GetEmployees(Guid companyId);
}