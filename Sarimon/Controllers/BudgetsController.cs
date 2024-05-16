namespace Curacaru.Backend.Controllers;

using Application.CQRS.Budgets;
using Core.DTO.Budget;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>Controller for budgets.</summary>
/// <param name="mediator">The CQRS mediator.</param>
[Authorize(Policy = Policy.Manager)]
[ApiController]
[Route("[controller]")]
public class BudgetsController(IMediator mediator) : ControllerBase

{
    /// <summary>Gets the list of budgets for the company</summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetBudgetsList([FromQuery] int page, [FromQuery] int pageSize = 20)
    {
        var budgetList = await mediator.Send(new GetBudgetListRequest(CompanyId, page, pageSize));
        return Ok(budgetList);
    }

    /// <summary>Gets the current budget for a customer.</summary>
    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCurrentBudget(Guid customerId)
    {
        var budget = await mediator.Send(new GetBudgetRequest(CompanyId, customerId));
        return Ok(budget);
    }

    /// <summary>Replaces the current budget for a customer.</summary>
    [HttpPut("{customerId:guid}")]
    public async Task<IActionResult> PutBudget([FromRoute] Guid customerId, [FromBody] PutBudgetDto budget)
    {
        await mediator.Send(new ReplaceBudgetRequest(CompanyId, customerId, budget));
        return Ok();
    }
}