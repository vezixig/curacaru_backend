namespace Curacaru.Backend.Core.DTO.Insurance;

using System.ComponentModel.DataAnnotations;

public class UpdateInsuranceDto
{
    /// <inheritdoc cref="Entities.Insurance.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Entities.Insurance.InstitutionCode" />
    public string InstitutionCode { get; set; } = "";

    /// <inheritdoc cref="Entities.Insurance.Name" />
    [Required]
    public string Name { get; set; } = "";

    /// <inheritdoc cref="Entities.Insurance.Street" />
    public string Street { get; set; } = "";

    /// <inheritdoc cref="Entities.Insurance.ZipCode" />
    public string ZipCode { get; set; } = "";
}