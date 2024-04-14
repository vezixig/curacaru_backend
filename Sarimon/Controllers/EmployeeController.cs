namespace Curacaru.Backend.Controllers;

using Application.CQRS.Employee;
using Core.DTO;
using Core.DTO.Employee;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Policy = Policy.Company)]
    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployee([FromRoute] string employeeId)
    {
        var employee = await mediator.Send(new EmployeeByIdRequest(CompanyId, AuthId, employeeId));
        return employee == null ? NotFound() : Ok(employee);
    }

    [Authorize(Policy = Policy.Company)]
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

    [Authorize(Policy = Policy.Company)]
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
        var updatedEmployee = await mediator.Send(new UpdateEmployeeRequest(CompanyId, UserId, employee));
        return Ok(updatedEmployee);
    }

    [Authorize(Policy = Policy.Company)]
    [HttpPost("change-password")]
    public async Task<IActionResult> UpdatePassword()
    {
        await mediator.Send(new ChangePasswordRequest(AuthId));
        return NoContent();
    }

    [Authorize(Policy = Policy.Company)]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto employee)
    {
        await mediator.Send(new UpdateProfileRequest(AuthId, employee));
        return NoContent();
    }
}