namespace Curacaru.Backend.Application.CQRS.Products;

using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

public class GetProductsListRequest : IRequest<List<Product>>
{
}

public class GetProductsListRequestHandler(IProductRepository productRepository) : IRequestHandler<GetProductsListRequest, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsListRequest request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProducts();
        return products;
    }
}