namespace Curacaru.Backend.Infrastructure.repositories;

using Core.Entities;

public interface IEmployeeRepository
{
    /// <summary>Checks if an employee with a given authId exists.</summary>
    /// <param name="authId">The auth id to check for.</param>
    /// <returns>True if an employee exists, otherwise false.</returns>
    public Task<Employee?> GetEmployeeByAuthId(string authId);
}