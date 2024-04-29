namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>A lookup for zip code to city.</summary>
public class ZipCity
{
    /// <summary>Gets or sets the name of the city.</summary>
    [Required]
    [MaxLength(30)]
    public string City { get; set; } = "";

    /// <summary>Gets or sets the zip code of the city.</summary>
    [Key]
    [MaxLength(5)]
    public string ZipCode { get; set; } = "";
}