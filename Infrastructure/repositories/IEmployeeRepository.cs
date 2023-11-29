namespace Curacaru.Backend.Infrastructure.repositories;

using Core.Entities;

public interface IEmployeeRepository
{
    /// <summary>Adds an employee to the database.</summary>
    public Task<Employee> AddEmployee(Employee employee);

    /// <summary>Adds an employee to the database.</summary>
    /// <param name="employee">The employee to add.</param>
    /// <returns>The added employee.</returns>
    public Task<Employee> AddEmployeeAsync(Employee employee);

    /// <summary>Checks if an employee with a given authId exists.</summary>
    /// <param name="authId">The auth id to check for.</param>
    /// <returns>True if an employee exists, otherwise false.</returns>
    public Task<Employee?> GetEmployeeByAuthId(string authId);

    /// <summary>Gets all employees of a company.</summary>
    /// <param name="companyId">The company id to get the employees for.</param>
    /// <returns>A list of employees.</returns>
    public Task<List<Employee>> GetEmployees(Guid companyId);
}