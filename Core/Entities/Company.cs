namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Company
{
    [MaxLength(11)]
    public string Bic { get; set; } = "";

    public decimal EmployeeSalary { get; set; }

    [MaxLength(32)]
    public string Iban { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    [MaxLength(9)]
    public string InstitutionCode { get; set; } = "";

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = "";

    [MaxLength(150)]
    public string OwnerName { get; set; } = "";

    public decimal PricePerHour { get; set; }

    public DateOnly RecognitionDate { get; set; }

    public decimal RideCosts { get; set; }

    public RideCostsType RideCostsType { get; set; } = RideCostsType.Inclusive;

    [MaxLength(150)]
    public string ServiceId { get; set; } = "";

    [MaxLength(150)]
    public string Street { get; set; } = "";

    [MaxLength(150)]
    public string TaxNumber { get; set; } = "";

    public ZipCity? ZipCity { get; set; } = new();

    [MaxLength(5)]
    public string? ZipCode { get; set; }
}