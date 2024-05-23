namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IProductRepository
{
    /// <summary>Gets a list of products by their ids</summary>
    /// <param name="productIds">The product ids.</param>
    /// <returns>A list of products.</returns>
    public Task<List<Product>> GetProducts(List<int> productIds);
}