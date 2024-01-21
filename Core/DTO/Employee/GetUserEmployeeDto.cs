namespace Curacaru.Backend.Core.DTO.Employee;

using Entities;
using Enums;

/// <summary>DTO to get an employee with some company info.</summary>
public class GetUserEmployeeDto
{
    /// <inheritdoc cref="Company.Id" />
    public Guid? CompanyId { get; set; }

    /// <inheritdoc cref="Company.Name" />
    public string CompanyName { get; set; } = "";

    /// <inheritdoc cref="Company.RideCostsType" />
    public RideCostsType CompanyRideCostsType { get; set; }

    /// <inheritdoc cref="Employee.FirstName" />
    public string FirstName { get; set; } = "";

    /// <inheritdoc cref="Employee.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Employee.IsManager" />
    public bool IsManager { get; set; }

    /// <inheritdoc cref="Employee.LastName" />
    public string LastName { get; set; } = "";
}