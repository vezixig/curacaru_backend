namespace Curacaru.Backend.Core.DTO;

using System.ComponentModel.DataAnnotations;

public class UpdateEmployeeDto
{
    public string Email { get; set; } = "";

    public string FirstName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public bool IsManager { get; set; }

    public string LastName { get; set; } = "";

    public string PhoneNumber { get; set; } = "";
}