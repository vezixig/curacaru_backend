namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Required]
    public string AuthId { get; set; } = "";

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