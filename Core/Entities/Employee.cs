namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>An employee of a company.</summary>
public class Employee
{
    /// <summary>Gets or sets the authentication id received from Auth0.</summary>
    [Required]
    [Length(30, 30)]
    public string AuthId { get; set; } = "";

    /// <summary>Gets or sets the company id.</summary>
    public Guid? CompanyId { get; set; }

    public string Email { get; set; } = "";

    public string FirstName { get; set; } = "";

    public string FullName => $"{FirstName} {LastName}".Trim();

    [Key]
    public Guid Id { get; set; }

    public bool IsManager { get; set; }

    public string LastName { get; set; } = "";

    public string PhoneNumber { get; set; } = "";
}