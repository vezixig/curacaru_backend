namespace Curacaru.Backend.Core.DTO.Insurance;

using Entities;

/// <summary>DTO to get an insurance.</summary>
/// <see cref="Insurance" />
public class GetInsuranceDto
{
    /// <summary>Gets or sets the city the insurance is located in.</summary>
    public string City { get; set; } = "";

    /// <inheritdoc cref="Entities.Insurance.CompanyId" />
    public Guid? CompanyId { get; set; }

    /// <inheritdoc cref="Insurance.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Insurance.InstitutionCode" />
    public string InstitutionCode { get; set; } = "";

    /// <inheritdoc cref="Insurance.Name" />
    public string Name { get; set; } = "";

    /// <inheritdoc cref="Insurance.Street" />
    public string Street { get; set; } = "";

    /// <inheritdoc cref="Insurance.ZipCode" />
    public string ZipCode { get; set; } = "";
}