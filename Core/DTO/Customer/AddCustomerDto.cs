namespace Curacaru.Backend.Core.DTO.Customer;

using Entities;
using Enums;

public class AddCustomerDto
{
    public Guid? AssociatedEmployeeId { get; set; }

    public DateOnly BirthDate { get; set; }

    public int CareLevel { get; set; }

    public List<int> DeclarationsOfAssignment { get; set; } = [];

    /// <inheritdoc cref="Customer.DoClearanceCareBenefit" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceReliefAmount" />
    public bool DoClearanceReliefAmount { get; set; }

    public string EmergencyContactName { get; set; } = "";

    public string EmergencyContactPhone { get; set; } = "";

    public string FirstName { get; set; } = "";

    public Guid? InsuranceId { get; set; }

    public InsuranceStatus? InsuranceStatus { get; set; }

    public string InsuredPersonNumber { get; set; } = "";

    public bool IsCareContractAvailable { get; set; }

    public string LastName { get; set; } = "";

    public string Phone { get; set; } = "";

    public string Street { get; set; } = "";

    public string? ZipCode { get; set; } = "";
}