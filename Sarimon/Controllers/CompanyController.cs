﻿namespace Curacaru.Backend.Controllers;

using Application.CQRS.Company;
using Core.DTO.Company;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>Controller for the company data.</summary>
/// <param name="mediator">The mediator.</param>
[Authorize(Policy = Policy.Manager)]
[ApiController]
[Route("[controller]")]
public class CompanyController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets the company data for the current user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetCompany()
    {
        var company = await mediator.Send(new CompanyRequest(CompanyId));
        return company == null ? NotFound() : Ok(company);
    }

    /// <summary>Updates the data of the company for the current user.</summary>
    [HttpPut]
    public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto companyData)
    {
        await mediator.Send(new UpdateCompanyRequest(CompanyId, companyData));
        return NoContent();
    }
}