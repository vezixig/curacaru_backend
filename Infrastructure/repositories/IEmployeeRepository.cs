namespace Curacaru.Backend.Infrastructure.repositories;

using Core.Entities;

public interface IEmployeeRepository
{
    /// <summary>Adds an employee to the database.</summary>
    /// <param name="employee">The employee to add.</param>
    /// <returns>The added employee.</returns>
    public Task<Employee> AddEmployeeAsync(Employee employee);

    /// <summary>Deletes an employee.</summary>
    /// <param name="employee">The employee to delete.</param>
    /// <returns>An awaitable task object.</returns>
    public Task DeleteEmployeeAsync(Employee employee);

    /// <summary>Checks if an email exists.</summary>
    /// <param name="email">The email to check for.</param>
    /// <returns>True if the email exists, otherwise false.</returns>
    public Task<bool> DoesEmailExistAsync(string email);

    /// <summary>Checks if an employee with a given authId exists.</summary>
    /// <param name="authId">The auth id to check for.</param>
    /// <returns>True if an employee exists, otherwise false.</returns>
    public Task<Employee?> GetEmployeeByAuthIdAsync(string authId);

    /// <summary>Gets an employee by id.</summary>
    /// <param name="employeeId">The employee id.</param>
    /// <param name="companyId">The company id for auth.</param>
    /// <returns>The employee.</returns>
    public Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, Guid companyId);

    /// <summary>Gets all employees of a company.</summary>
    /// <param name="companyId">The company id to get the employees for.</param>
    /// <returns>A list of employees.</returns>
    public Task<List<Employee>> GetEmployeesAsync(Guid companyId);

    /// <summary>Updates an employee.</summary>
    /// <param name="currentEmploye">The modified employee.</param>
    /// <returns>The updated employee.</returns>
    public Task<Employee> UpdateEmployeeAsync(Employee currentEmploye);
}