namespace Curacaru.Backend.Core.DTO.Customer;

using System.ComponentModel.DataAnnotations;
using Entities;
using Enums;
using Insurance;

/// <summary>DTO for getting a customer.</summary>
/// <seealso cref="Customer" />
public class GetCustomerDto
{
    public Guid? AssociatedEmployeeId { get; set; }

    public DateOnly BirthDate { get; set; }

    public int CareLevel { get; set; }

    public string City { get; set; } = "";

    [Required]
    public Guid CompanyId { get; set; }

    public List<int> DeclarationsOfAssignment { get; set; } = [];

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

    public GetInsuranceDto? Insurance { get; set; }

    public Guid? InsuranceId { get; set; }

    public InsuranceStatus? InsuranceStatus { get; set; }

    public string InsuredPersonNumber { get; set; } = "";

    public bool IsCareContractAvailable { get; set; }

    public string LastName { get; set; } = "";

    public string Phone { get; set; } = "";

    /// <inheritdoc cref="Customer.Salutation" />
    public Gender Salutation { get; set; }

    public string Street { get; set; } = "";

    public string? ZipCode { get; set; } = "";
}