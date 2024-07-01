namespace Curacaru.Backend.Controllers;

using Application.CQRS.Customer;
using Core.DTO.Customer;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class CustomerController(ISender mediator) : ControllerBase
{
    [Authorize(Policy = Policy.Manager)]
    [HttpPost("new")]
    public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDto customer)
    {
        var newCustomer = await mediator.Send(new AddCustomerRequest(CompanyId, customer));
        return CreatedAtAction(nameof(GetCustomer), new { customerId = newCustomer.Id }, newCustomer);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpDelete("{customerId:guid}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid customerId, [FromQuery] bool deleteOpenAppointments, [FromQuery] bool deleteBudgets)
    {
        await mediator.Send(new SetCustomerInactiveRequest(AuthUser, customerId, deleteOpenAppointments, deleteBudgets));
        return NoContent();
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpDelete("{customerId:guid}/prospect")]
    public async Task<IActionResult> DeleteProspectCustomer([FromRoute] Guid customerId)
    {
        await mediator.Send(new DeleteCustomerRequest(AuthUser, customerId));
        return NoContent();
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid customerId)
    {
        var customer = await mediator.Send(new CustomerRequest(AuthUser, customerId));
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int page,
        [FromQuery] CustomerStatus status,
        [FromQuery] Guid? employeeId,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? orderBy = null)
    {
        var customers = await mediator.Send(new CustomerListRequest(AuthUser, page, pageSize, employeeId, status, orderBy == "date"));
        return Ok(customers);
    }

    [HttpGet("{customerId:guid}/budget")]
    public async Task<IActionResult> GetCustomerWithBudget([FromRoute] Guid customerId)
    {
        var customer = await mediator.Send(new CustomerWithBudgetRequest(AuthUser, customerId));
        return Ok(customer);
    }

    [HttpGet("list/minimal")]
    public async Task<IActionResult> GetMinimalCustomerList(
        [FromQuery] InsuranceStatus? insuranceStatus,
        [FromQuery] int? assignmentDeclarationYear)
    {
        var customers = await mediator.Send(
            new MinimalCustomerListRequest(AuthUser, insuranceStatus, assignmentDeclarationYear));
        return Ok(customers);
    }

    [HttpGet("list/minimal-deployment-reports")]
    public async Task<IActionResult> GetMinimalCustomerList()
    {
        var customers = await mediator.Send(
            new MinimalCustomerListForDeploymentReportsRequest(AuthUser));
        return Ok(customers);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto customer)
    {
        var updatedCustomer = await mediator.Send(new UpdateCustomerRequest(AuthUser, customer));
        return Ok(updatedCustomer);
    }
}