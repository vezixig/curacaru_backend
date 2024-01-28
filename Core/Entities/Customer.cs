namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Customer
{
    public Employee? AssociatedEmployee { get; set; }

    public Guid? AssociatedEmployeeId { get; set; }

    public DateOnly BirthDate { get; set; }

    [Range(1, 5)]
    public int CareLevel { get; set; }

    [Required]
    public Guid CompanyId { get; set; }

    public List<int> DeclarationsOfAssignment { get; set; } = [];

    /// <summary>Gets or sets a value indicating whether clearance can be done through care benefit in kind.</summary>
    /// <see href="https://www.bundesgesundheitsministerium.de/pflegedienst-und-pflegesachleistungen" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through preventive care.</summary>
    /// <see href="https://www.gesetze-im-internet.de/sgb_11/__39.html" />
    public bool DoClearancePreventiveCare { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through the relief amount.</summary>
    /// <see href="https://www.bundesgesundheitsministerium.de/entlastungsbetrag" />
    public bool DoClearanceReliefAmount { get; set; }

    public string EmergencyContactName { get; set; } = "";

    public string EmergencyContactPhone { get; set; } = "";

    public string FirstName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public Insurance? Insurance { get; set; }

    public Guid? InsuranceId { get; set; }

    public InsuranceStatus? InsuranceStatus { get; set; }

    public string InsuredPersonNumber { get; set; } = "";

    public bool IsCareContractAvailable { get; set; }

    public string LastName { get; set; } = "";

    /// <summary>Gets or sets the latitude of the customer's address.</summary>
    public decimal Latitude { get; set; }

    /// <summary>Gets or sets the longitude of the customer's address.</summary>
    public decimal Longitude { get; set; }

    public string Phone { get; set; } = "";

    /// <summary>Gets or sets the salutation of the customer based on the gender.</summary>
    public Gender Salutation { get; set; }

    public string Street { get; set; } = "";

    public ZipCity? ZipCity { get; set; }

    public string? ZipCode { get; set; } = "";
}