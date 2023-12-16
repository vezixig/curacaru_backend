namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>A health insurance company.</summary>
public class Insurance
{
    [Required]
    public Guid CompanyId { get; set; }

    [Key]
    public Guid Id { get; set; }

    public string InstitutionCode { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";
}