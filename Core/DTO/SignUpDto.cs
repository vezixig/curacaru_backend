namespace Curacaru.Backend.Core.DTO;

/// <summary>DTO used for signing up a new user as first employee of a new company.</summary>
public class SignUpDto
{
    /// <summary>The name of the company.</summary>
    public string CompanyName { get; set; } = "";

    /// <summary>The first name of the user/employee.</summary>
    public string FirstName { get; set; } = "";

    /// <summary>The last name of the user/employee.</summary>
    public string LastName { get; set; } = "";
}