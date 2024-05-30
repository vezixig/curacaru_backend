namespace Curacaru.Backend.Infrastructure.Repositories;

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
    /// <param name="companyId">The company id for auth.</param>
    /// <param name="employeeId">The employee id.</param>
    /// <returns>The employee.</returns>
    public Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId, bool withTracking = false);

    /// <summary>Gets all employees of a company.</summary>
    /// <param name="companyId">The company id to get the employees for.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A list of employees.</returns>
    public Task<List<Employee>> GetEmployeesAsync(Guid companyId, int? page = null, int? pageSize = null);

    /// <summary>Gets the number of employees of a company.</summary>
    /// <param name="companyId">The company id to get the employee count for.</param>
    public Task<int> GetEmployeesCountAsync(Guid companyId);

    /// <summary>Updates an employee.</summary>
    /// <param name="currentEmployee">The modified employee.</param>
    /// <returns>The updated employee.</returns>
    public Task<Employee> UpdateEmployeeAsync(Employee currentEmployee);
}