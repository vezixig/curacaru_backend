namespace Curacaru.Backend.Core.DTO;

using System.ComponentModel.DataAnnotations;

/// <summary>DTO to return a list of customers with only basic information.</summary>
public class GetCustomerListEntryDto
{
    public string AssociatedEmployeeName { get; set; } = "";

    public string City { get; set; } = "";

    public string FirstName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public string LastName { get; set; } = "";

    public string Phone { get; set; } = "";

    public string ZipCode { get; set; } = "";
}