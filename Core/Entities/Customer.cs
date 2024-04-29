namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Customer
{
    /// <summary>Gets or sets the appointments of the customer.</summary>
    public List<Appointment> Appointments { get; set; } = [];

    /// <summary>Gets or sets the assignment declarations of the customer.</summary>
    public List<AssignmentDeclaration> AssignmentDeclarations { get; set; } = [];

    public Employee? AssociatedEmployee { get; set; }

    public Guid? AssociatedEmployeeId { get; set; }

    public DateOnly BirthDate { get; set; }

    [Range(1, 5)]
    public int CareLevel { get; set; }

    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through care benefit in kind.</summary>
    /// <see href="https://www.bundesgesundheitsministerium.de/pflegedienst-und-pflegesachleistungen" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through preventive care.</summary>
    /// <see href="https://www.gesetze-im-internet.de/sgb_11/__39.html" />
    public bool DoClearancePreventiveCare { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through the relief amount.</summary>
    /// <see href="https://www.bundesgesundheitsministerium.de/entlastungsbetrag" />
    public bool DoClearanceReliefAmount { get; set; }

    /// <summary>Gets or sets a value indicating whether clearance can be done through self-payment.</summary>
    public bool DoClearanceSelfPayment { get; set; }

    /// <summary>Gets or sets the emergency contact name.</summary>
    [MaxLength(150)]
    public string EmergencyContactName { get; set; } = "";

    /// <summary>Gets or sets the emergency contact phone number.</summary>
    [MaxLength(50)]
    public string EmergencyContactPhone { get; set; } = "";

    /// <summary>Gets or sets the first name.</summary>
    [MaxLength(50)]
    public string FirstName { get; set; } = "";

    public string FullName => $"{FirstName} {LastName}".Trim();

    public string FullNameReverse => $"{LastName}, {FirstName}".Trim();

    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets the insurance the customer is insured at.</summary>
    public Insurance? Insurance { get; set; }

    /// <summary>Gets or sets the id of insurance the customer is insured at.</summary>
    public Guid? InsuranceId { get; set; }

    /// <summary>Gets or sets the insurance status.</summary>
    public InsuranceStatus? InsuranceStatus { get; set; }

    /// <summary>Gets or sets the insured person number of the customer.</summary>
    [MaxLength(10)]
    public string InsuredPersonNumber { get; set; } = "";

    /// <summary>Gets or sets the last name.</summary>
    [MaxLength(50)]
    public string LastName { get; set; } = "";

    /// <summary>Gets or sets the phone number of the customer.</summary>
    [MaxLength(50)]
    public string Phone { get; set; } = "";

    /// <summary>Gets or sets the salutation of the customer based on the gender.</summary>
    public Gender Salutation { get; set; }

    /// <summary>Gets or sets the street of the customer's address.</summary>
    [MaxLength(150)]
    public string Street { get; set; } = "";

    public ZipCity? ZipCity { get; set; }

    [MaxLength(5)]
    public string? ZipCode { get; set; } = "";
}