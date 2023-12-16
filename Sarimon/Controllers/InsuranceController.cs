namespace Curacaru.Backend.Controllers;

using Application.CQRS.Insurance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class InsuranceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> SearchInsurance([FromQuery] string search)
    {
        var result = await _mediator.Send(new InsuranceSearchRequest(search));
        return Ok(result);
    }
}