﻿namespace Curacaru.Backend.Core.Entities;

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

    public List<int> DeclarationsOfAssignment { get; set; } = new();

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

    public string Phone { get; set; } = "";

    public string Street { get; set; } = "";

    public ZipCity? ZipCity { get; set; }

    public string? ZipCode { get; set; } = "";
}