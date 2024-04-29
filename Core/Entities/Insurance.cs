namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>A health insurance company.</summary>
public class Insurance
{
    /// <summary>Gets or sets the company the insurance is associated with.</summary>
    /// <remarks>NULL if it's a default insurance,</remarks>
    public Guid? CompanyId { get; set; }

    /// <summary>Gets or sets the id of the insurance.</summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets the institution code.</summary>
    [MaxLength(9)]
    public string InstitutionCode { get; set; } = "";

    /// <summary>Gets or sets the name of the insurance.</summary>
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = "";

    /// <summary>Gets or sets the street the insurance is located in.</summary>
    [MaxLength(150)]
    public string Street { get; set; } = "";

    /// <summary>Gets or sets the zip code the insurance is located in.</summary>
    /// <remarks>This is a lookup property.</remarks>
    public ZipCity? ZipCity { get; set; }

    /// <summary>Gets or sets the zip code the insurance is located in.</summary>
    [MaxLength(5)]
    public string? ZipCode { get; set; } = null;
}