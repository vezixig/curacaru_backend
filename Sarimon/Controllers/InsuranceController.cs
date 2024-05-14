namespace Curacaru.Backend.Controllers;

using Application.CQRS.Insurance;
using Core.DTO.Insurance;
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
    [HttpPost("new")]
    public async Task<IActionResult> AddInsurance([FromBody] AddInsuranceDto insurance)
    {
        var result = await mediator.Send(new AddInsuranceRequest(CompanyId, insurance));
        return Ok(result);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInsurance([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteInsuranceRequest(CompanyId, id));
        return NoContent();
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpGet("list")]
    public async Task<IActionResult> ListInsurances([FromQuery] int page, [FromQuery] int pageSize = 20)
    {
        var result = await mediator.Send(new InsuranceListRequest(CompanyId, page, pageSize));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> SearchInsurance([FromQuery] string search)
    {
        var result = await mediator.Send(new InsuranceSearchRequest(CompanyId, search));
        return Ok(result);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> SearchInsurance([FromRoute] Guid id)
    {
        var result = await mediator.Send(new InsuranceRequest(CompanyId, id));
        return Ok(result);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateInsurance([FromBody] UpdateInsuranceDto insurance)
    {
        var result = await mediator.Send(new UpdateInsuranceRequest(CompanyId, insurance));
        return Ok(result);
    }
}