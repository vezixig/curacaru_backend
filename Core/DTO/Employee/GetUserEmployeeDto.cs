namespace Curacaru.Backend.Core.DTO;

public class GetUserEmployeeDto
{
    public Guid? CompanyId { get; set; }

    public string CompanyName { get; set; } = "";

    public string FirstName { get; set; } = "";

    public bool IsManager { get; set; }

    public string LastName { get; set; } = "";
}