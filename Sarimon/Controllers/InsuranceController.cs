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
    [Authorize(Policy = Policy.Manager)]
    [HttpGet("list")]
    public async Task<IActionResult> ListInsurances()
    {
        var result = await mediator.Send(new InsuranceListRequest(CompanyId));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> SearchInsurance([FromQuery] string search)
    {
        var result = await mediator.Send(new InsuranceSearchRequest(search));
        return Ok(result);
    }
}