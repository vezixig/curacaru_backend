namespace Curacaru.Backend.Core.Models;

/// <summary>Model to hold data of the authorized user.</summary>
public class User
{
    /// <summary>Gets the company id.</summary>
    public Guid CompanyId { get; init; }

    /// <summary>Gets the employee id.</summary>
    public Guid EmployeeId { get; init; }

    /// <summary>Gets a value indicating whether the user is a manager.</summary>
    public bool IsManager { get; init; }
}