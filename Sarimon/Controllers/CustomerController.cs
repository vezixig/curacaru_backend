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
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid customerId)
    {
        await mediator.Send(new DeleteCustomerRequest(AuthId, customerId));
        return NoContent();
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid customerId)
    {
        var customer = await mediator.Send(new CustomerRequest(CompanyId, customerId));
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await mediator.Send(new CustomerListRequest(CompanyId, AuthId));
        return Ok(customers);
    }

    [HttpGet("{customerId:guid}/budget")]
    public async Task<IActionResult> GetCustomerWithBudget([FromRoute] Guid customerId)
    {
        var customer = await mediator.Send(new CustomerWithBudgetRequest(CompanyId, AuthId, customerId));
        return Ok(customer);
    }

    [HttpGet("list/minimal")]
    public async Task<IActionResult> GetMinimalCustomerList(
        [FromQuery] InsuranceStatus? insuranceStatus,
        [FromQuery] int? assignmentDeclarationYear)
    {
        var customers = await mediator.Send(
            new MinimalCustomerListRequest(CompanyId, AuthId, insuranceStatus, assignmentDeclarationYear));
        return Ok(customers);
    }

    [HttpGet("list/minimal-deployment-reports")]
    public async Task<IActionResult> GetMinimalCustomerList()
    {
        var customers = await mediator.Send(
            new MinimalCustomerListForDeploymentReportsRequest(CompanyId, AuthId));
        return Ok(customers);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto customer)
    {
        var updatedCustomer = await mediator.Send(new UpdateCustomerRequest(customer, CompanyId, AuthId));
        return Ok(updatedCustomer);
    }
}