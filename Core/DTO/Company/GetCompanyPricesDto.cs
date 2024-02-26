namespace Curacaru.Backend.Core.DTO.Company;

using Enums;

/// <summary>A DTO to get the prices of the company.</summary>
public class GetCompanyPricesDto
{
    public decimal PricePerHour { get; set; }

    public decimal RideCosts { get; set; }

    public RideCostsType RideCostsType { get; set; }
}