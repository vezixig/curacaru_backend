namespace Curacaru.Backend.Core.DTO;

using System.ComponentModel.DataAnnotations;
using Entities;

/// <summary>DTO to return a list of customers with only basic information.</summary>
public class GetCustomerListEntryDto
{
    /// <inheritdoc cref="Customer.AssociatedEmployeeId" />
    public Guid AssociatedEmployeeId { get; set; }

    public string AssociatedEmployeeName { get; set; } = "";

    public string City { get; set; } = "";

    public string FirstName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public string LastName { get; set; } = "";

    public string Phone { get; set; } = "";

    public string ZipCode { get; set; } = "";
}