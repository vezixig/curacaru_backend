namespace Curacaru.Backend.Controllers;

using Application.CQRS.Address;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class AddressController(ISender mediator) : ControllerBase
{
    [HttpGet("city/{zipCode}")]
    public async Task<IActionResult> GetCity([FromRoute] string zipCode)
    {
        var city = await mediator.Send(new CityByZipCodeRequest(zipCode));
        return city == null ? NotFound() : Ok(city);
    }
}