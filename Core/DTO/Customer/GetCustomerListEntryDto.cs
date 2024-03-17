namespace Curacaru.Backend.Core.DTO.Customer;

using System.ComponentModel.DataAnnotations;
using Entities;

/// <summary>DTO to return a list of customers with only basic information.</summary>
public class GetCustomerListEntryDto
{
    /// <inheritdoc cref="Customer.AssociatedEmployeeId" />
    public Guid AssociatedEmployeeId { get; set; }

    /// <summary>Gets or sets the name of the associated employee.</summary>
    public string AssociatedEmployeeName { get; set; } = "";

    /// <summary>Gets or sets the city of the customer.</summary>
    public string City { get; set; } = "";

    /// <inheritdoc cref="Customer.FirstName" />
    public string FirstName { get; set; } = "";

    /// <inheritdoc cref="Customer.Id" />
    [Key]
    public Guid Id { get; set; }

    /// <inheritdoc cref="Customer.LastName" />
    public string LastName { get; set; } = "";

    /// <inheritdoc cref="Customer.Phone" />
    public string Phone { get; set; } = "";

    /// <inheritdoc cref="Customer.Street" />
    public string Street { get; set; } = "";

    /// <inheritdoc cref="Customer.ZipCode" />
    public string ZipCode { get; set; } = "";
}