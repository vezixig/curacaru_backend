namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Company
{
    public string Bic { get; set; } = "";

    public decimal EmployeeSalary { get; set; }

    public string Iban { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public string InstitutionCode { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    public string OwnerName { get; set; } = "";

    public decimal PricePerHour { get; set; }

    public DateOnly RecognitionDate { get; set; }

    public decimal RideCosts { get; set; }

    public RideCostsType RideCostsType { get; set; } = RideCostsType.Inclusive;

    public string ServiceId { get; set; } = "";

    public string Street { get; set; } = "";

    public string TaxNumber { get; set; } = "";

    public ZipCity? ZipCity { get; set; } = new();

    public string? ZipCode { get; set; }
}