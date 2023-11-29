namespace Curacaru.Backend.Core.DTO;

public class AddEmployeeDto
{
    public string Email { get; set; } = "";

    public string FirstName { get; set; } = "";

    public bool IsManager { get; set; }

    public string LastName { get; set; } = "";

    public string PhoneNumber { get; set; } = "";
}