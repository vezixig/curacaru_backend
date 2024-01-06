namespace Curacaru.Backend.Core.DTO.Company;

using Enums;

public class GetCompanyDto
{
    public string Bic { get; set; } = "";

    public decimal EmployeeSalary { get; set; }

    public string Iban { get; set; } = "";

    public string InstitutionCode { get; set; } = "";

    public string Name { get; set; } = "";

    public string OwnerName { get; set; } = "";

    public decimal PricePerHour { get; set; }

    public DateOnly RecognitionDate { get; set; }

    public decimal RideCosts { get; set; }

    public RideCostsType RideCostsType { get; set; }

    public string ServiceId { get; set; } = "";

    public string Street { get; set; } = "";

    public string TaxNumber { get; set; } = "";

    public string ZipCode { get; set; } = "";
}