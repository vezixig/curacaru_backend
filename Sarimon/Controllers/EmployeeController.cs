namespace Curacaru.Backend.Controllers;

using System.Security.Claims;
using Application.CQRS.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;

    private readonly IMediator _mediator;

    public EmployeeController(ILogger<EmployeeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeByAuthId()
    {
        var authId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var employee = await _mediator.Send(new EmployeeExistsByAuthIdQuery(authId));
        return employee == null ? NotFound() : Ok(employee);
    }
}