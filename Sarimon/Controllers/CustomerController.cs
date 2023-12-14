namespace Curacaru.Backend.Controllers;

using Application.CQRS.Customer;
using Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("new")]
    public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDto customer)
    {
        if (CompanyId == null) return Forbid();
        if (!IsManager) return Forbid("Nur Manager dürfen neue Kunden anlegen.");

        var newCustomer = await _mediator.Send(new AddCustomerRequest(CompanyId.Value, customer));
        return CreatedAtAction(nameof(GetCustomer), new { newCustomer.Id }, newCustomer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        => Ok();

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer([FromRoute] string customerId)
    {
        var customer = await _mediator.Send(new CustomerRequest(CompanyId.Value, customerId));
        return Ok(customer);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCustomers()
    {
        if (CompanyId == null) return Forbid();

        var customers = await _mediator.Send(new CustomerListRequest(CompanyId.Value));
        return Ok(customers);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto customer)
    {
        if (CompanyId == null) return Forbid();
        if (!IsManager) return Forbid("Nur Manager dürfen Kunden bearbeiten.");

        var updatedCustomer = await _mediator.Send(new UpdateCustomerRequest(customer, CompanyId.Value, AuthId));
        return Ok(updatedCustomer);
    }
}