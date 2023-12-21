namespace Curacaru.Backend.Controllers;

using Application.CQRS.Customer;
using Core.DTO;
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
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] string customerId)
    {
        await mediator.Send(new DeleteCustomerRequest(AuthId, Guid.Parse(customerId)));
        return NoContent();
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer([FromRoute] string customerId)
    {
        var customer = await mediator.Send(new CustomerRequest(CompanyId, customerId));
        return Ok(customer);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await mediator.Send(new CustomerListRequest(CompanyId, AuthId));
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