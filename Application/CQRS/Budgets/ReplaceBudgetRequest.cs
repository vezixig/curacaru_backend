namespace Curacaru.Backend.Application.CQRS.Budgets;

using Core.DTO.Budget;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Services;

/// <summary>Request to replace the current budget of a customer.</summary>
public class ReplaceBudgetRequest(Guid companyId, Guid customerId, PutBudgetDto budget) : IRequest
{
    /// <summary>Gets the budget.</summary>
    public PutBudgetDto Budget { get; } = budget;

    /// <summary>Gets the company id.</summary>
    public Guid CompanyId { get; } = companyId;

    /// <summary>Gets the customer id.</summary>
    public Guid CustomerId { get; } = customerId;
}

internal class ReplaceBudgetRequestHandler(IBudgetRepository budgetRepository, ICustomerRepository customerRepository, IDatabaseService databaseService)
    : IRequestHandler<ReplaceBudgetRequest>
{
    public async Task Handle(ReplaceBudgetRequest request, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetCurrentBudgetAsync(request.CompanyId, request.CustomerId);
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId)
                       ?? throw new BadRequestException("Kunde wurde nicht gefunden");

        if (budget == null || budget.Year != DateTime.Now.Year || budget.Month != DateTime.Now.Month)
            budget = new Budget
            {
                CompanyId = request.CompanyId,
                Customer = customer,
                CustomerId = request.CustomerId,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };

        var lastYearBudget = await budgetRepository.GetLastYearBudgetAsync(request.CompanyId, request.CustomerId)
                             ?? new Budget
                             {
                                 CompanyId = request.CompanyId,
                                 Customer = customer,
                                 CustomerId = request.CustomerId,
                                 Month = 12,
                                 Year = DateTime.Now.Year - 1
                             };

        // Calculate the relief amount for the current year and the remaining relief amount from last year
        var maxCurrentReliefAmount = BudgetService.MonthlyReliefAmountRaise * DateTime.Now.Month;

        if (request.Budget.ReliefAmount > maxCurrentReliefAmount + BudgetService.MonthlyReliefAmountRaise * 12)
            throw new BadRequestException("Entlastungsbetrag übersteigt die maximal mögliche Summe");

        lastYearBudget.ReliefAmount = request.Budget.ReliefAmount < maxCurrentReliefAmount
            ? 0
            : request.Budget.ReliefAmount - maxCurrentReliefAmount;

        budget.ReliefAmount = request.Budget.ReliefAmount - lastYearBudget.ReliefAmount;
        budget.SelfPayRaise = request.Budget.SelfPayRaise;
        budget.SelfPayAmount = request.Budget.SelfPayAmount;
        budget.CareBenefitAmount = request.Budget.CareBenefitAmount;
        budget.PreventiveCareAmount = request.Budget.PreventiveCareAmount;

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            await (budget.Id == Guid.Empty ? budgetRepository.AddBudgetAsync(budget) : budgetRepository.UpdateBudgetAsync(budget));
            await (lastYearBudget.Id == Guid.Empty ? budgetRepository.AddBudgetAsync(lastYearBudget) : budgetRepository.UpdateBudgetAsync(lastYearBudget));
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}