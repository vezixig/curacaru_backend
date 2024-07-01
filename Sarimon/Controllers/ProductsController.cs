namespace Curacaru.Backend.Controllers;

using Application.CQRS.Products;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsList()
    {
        var products = await mediator.Send(new GetProductsListRequest());
        return Ok(products);
    }
}