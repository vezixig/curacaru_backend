namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

/// <summary>Repository for <see cref="Budget" />.</summary>
public interface IBudgetRepository
{
    /// <summary>Gets a specific budget for a customer.</summary>
    Task<Budget?> GetBudgetAsync(
        Guid companyId,
        Guid customerId,
        int year,
        int month);

    /// <summary>Gets the list of current budgets for a company.</summary>
    Task<List<Budget>> GetBudgetListAsync(Guid companyId);

    /// <summary>Gets the current budget for a customer.</summary>
    Task<Budget?> GetCurrentBudgetAsync(Guid companyId, Guid customerId);
}