namespace Curacaru.Backend.Core.DTO;

using Entities;

public class GetEmployeeBase
{
    /// <inheritdoc cref="Employee.Id" />
    public Guid Id { get; set; }

    /// <summary>Concatenation of <see cref="Employee.FirstName" /> and <see cref="Employee.LastName" /></summary>
    public string Name { get; set; }
}