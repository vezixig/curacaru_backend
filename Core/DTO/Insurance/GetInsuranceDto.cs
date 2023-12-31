namespace Curacaru.Backend.Core.DTO.Insurance;

using Entities;

/// <summary>DTO to get an insurance.</summary>
/// <see cref="Insurance" />
public class GetInsuranceDto
{
    /// <inheritdoc cref="Insurance.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Insurance.InstitutionCode" />
    public string InstitutionCode { get; set; } = "";

    /// <inheritdoc cref="Insurance.Name" />
    public string Name { get; set; } = "";
}