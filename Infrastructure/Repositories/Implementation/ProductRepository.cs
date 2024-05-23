namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class ProductRepository(DataContext dataContext) : IProductRepository
{
    public Task<List<Product>> GetProducts(List<int> productIds)
        => dataContext.Products.Where(product => productIds.Contains(product.Id)).AsTracking().ToListAsync();
}