namespace Curacaru.Backend.Controllers;

using Application.CQRS.Employee;
using Core.DTO;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class EmployeeController(ISender mediator) : ControllerBase
{
    [Authorize(Policy = Policy.Manager)]
    [HttpPost("new")]
    public async Task<IActionResult> AddEmployee(AddEmployeeDto employee)
    {
        var newEmployee = await mediator.Send(new AddEmployeeRequest(CompanyId, employee));
        return CreatedAtAction(nameof(GetEmployeeByAuthId), new { AuthId }, newEmployee);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
    {
        await mediator.Send(new DeleteEmployeeRequest(AuthId, employeeId));
        return NoContent();
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployee([FromRoute] string employeeId)
    {
        var employee = await mediator.Send(new EmployeeByIdRequest(CompanyId, employeeId));
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpGet("baselist")]
    public async Task<IActionResult> GetEmployeeBaseList()
    {
        var employees = await mediator.Send(new EmployeeBaseListRequest(CompanyId));
        return Ok(employees);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeByAuthId()
    {
        var employee = await mediator.Send(new EmployeeByAuthIdRequest(AuthId));
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await mediator.Send(new EmployeeListRequest(CompanyId));
        return Ok(employees);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto employee)
    {
        var updatedEmployee = await mediator.Send(new UpdateEmployeeRequest(CompanyId, employee));
        return Ok(updatedEmployee);
    }
}