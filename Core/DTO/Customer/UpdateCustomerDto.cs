namespace Curacaru.Backend.Core.DTO.Customer;

using Entities;
using Enums;

public class UpdateCustomerDto
{
    public Guid? AssociatedEmployeeId { get; set; }

    public DateOnly BirthDate { get; set; }

    public int CareLevel { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceCareBenefit" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <inheritdoc cref="Customer.DoClearancePreventiveCare" />
    public bool DoClearancePreventiveCare { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceReliefAmount" />
    public bool DoClearanceReliefAmount { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceSelfPayment" />
    public bool DoClearanceSelfPayment { get; set; }

    public string EmergencyContactName { get; set; } = "";

    public string EmergencyContactPhone { get; set; } = "";

    public string FirstName { get; set; } = "";

    public Guid Id { get; set; }

    public Guid? InsuranceId { get; set; }

    public InsuranceStatus? InsuranceStatus { get; set; }

    public string InsuredPersonNumber { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Phone { get; set; } = "";

    /// <summary>Gets or sets the ids of the products the customer is interested in.</summary>
    public List<int> Products { get; set; } = [];

    /// <inheritdoc cref="Customer.Salutation" />
    public Gender Salutation { get; set; }

    /// <inheritdoc cref="Customer.Status" />
    public CustomerStatus? Status { get; set; }

    public string Street { get; set; } = "";

    public string? ZipCode { get; set; } = "";
}