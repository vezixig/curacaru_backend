namespace Curacaru.Backend.Controllers;

using Application.CQRS.Company;
using Core.DTO.Company;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>Controller for the company data.</summary>
/// <param name="mediator">The mediator.</param>
[ApiController]
[Route("[controller]")]
public class CompanyController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets the company data for the current user.</summary>
    [Authorize(Policy = Policy.Manager)]
    [HttpGet]
    public async Task<IActionResult> GetCompany()
    {
        var company = await mediator.Send(new CompanyRequest(CompanyId));
        return company == null ? NotFound() : Ok(company);
    }

    /// <summary>Gets the prices of the company.</summary>
    [Authorize(Policy = Policy.Company)]
    [HttpGet("prices")]
    public async Task<IActionResult> GetPrices()
    {
        var prices = await mediator.Send(new CompanyPricesRequest(CompanyId));
        return Ok(prices);
    }

    /// <summary>Updates the data of the company for the current user.</summary>
    [Authorize(Policy = Policy.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto companyData)
    {
        await mediator.Send(new UpdateCompanyRequest(CompanyId, companyData));
        return NoContent();
    }
}