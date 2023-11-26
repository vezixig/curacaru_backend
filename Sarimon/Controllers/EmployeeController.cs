namespace Curacaru.Backend.Controllers;

using Application.CQRS.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetEmployeeByAuthId()
    {
        var employee = await _mediator.Send(new EmployeeExistsByAuthIdQuery(AuthId));
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _mediator.Send(new EmployeeListQuery(AuthId));
        return Ok(employees);
    }
}