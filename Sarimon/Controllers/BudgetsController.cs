namespace Curacaru.Backend.Controllers;

using Application.CQRS.Budgets;
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
    /// <summary>Get the list of budgets for the company</summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetBudgets()
    {
        var budgetList = await mediator.Send(new GetBudgetListRequest(CompanyId));
        return Ok(budgetList);
    }

    /// <summary>Get the current budget for a customer.</summary>
    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCurrentBudget(Guid customerId)
    {
        var budget = await mediator.Send(new GetBudgetRequest(CompanyId, customerId));
        return Ok(budget);
    }
}