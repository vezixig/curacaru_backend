namespace Curacaru.Backend.Controllers;

using Application.CQRS.Customer;
using Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        => Ok();

    [HttpGet("list")]
    public async Task<IActionResult> GetCustomers()
    {
        if (CompanyId == null) return Forbid();

        var customers = await _mediator.Send(new CustomerListRequest(CompanyId.Value));
        return Ok(customers);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto customer)
        => Ok();
}