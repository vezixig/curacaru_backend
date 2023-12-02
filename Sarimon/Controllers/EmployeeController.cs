namespace Curacaru.Backend.Controllers;

using Application.CQRS.Employee;
using Core.DTO;
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

    [HttpPost("new")]
    public async Task<IActionResult> AddEmployee(AddEmployeeDto employee)
    {
        var newEmployee = await _mediator.Send(new AddEmployeeRequest(AuthId, employee));
        return CreatedAtAction(nameof(GetEmployeeByAuthId), new { AuthId }, newEmployee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteEmployeeRequest(AuthId, id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee([FromRoute] string id)
    {
        var employee = await _mediator.Send(new EmployeeByIdRequest(AuthId, id));
        return employee == null ? NotFound() : Ok(employee);
    }

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

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto employee)
    {
        var updatedEmployee = await _mediator.Send(new UpdateEmployeeRequest(AuthId, employee));
        return Ok(updatedEmployee);
    }
}