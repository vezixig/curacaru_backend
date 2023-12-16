namespace Curacaru.Backend.Controllers;

using Application.CQRS.Address;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("city/{zipCode}")]
    public async Task<IActionResult> GetCity([FromRoute] string zipCode)
    {
        var city = await _mediator.Send(new CityByZipCodeRequest(zipCode));
        return city == null ? NotFound() : Ok(city);
    }
}