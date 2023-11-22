namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = "";
}