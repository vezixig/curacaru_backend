namespace Curacaru.Backend.Controllers;

using Application.CQRS.Insurance;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class InsuranceController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SearchInsurance([FromQuery] string search)
    {
        var result = await mediator.Send(new InsuranceSearchRequest(search));
        return Ok(result);
    }
}