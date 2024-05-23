namespace Curacaru.Backend.Core.Entities;

/// <summary>Represents a product assigned to a customer.</summary>
public class Product

{
    /// <summary>Gets or sets the id of the product.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the name of the product.</summary>
    public string Name { get; set; } = null!;
}