namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class BudgetRepository(DataContext dataContext) : IBudgetRepository
{
    public Task<Budget?> GetBudgetAsync(
        Guid companyId,
        Guid customerId,
        int year,
        int month)
        => dataContext.Budgets.FirstOrDefaultAsync(o => o.CustomerId == customerId && o.CompanyId == companyId && o.Year == year && o.Month == month);

    public Task<List<Budget>> GetBudgetListAsync(Guid companyId)
        => dataContext.Budgets
            .Include(c => c.Customer)
            .Where(o => o.CompanyId == companyId && o.Year == DateTime.Now.Year && o.Month == DateTime.Now.Month)
            .ToListAsync();

    public Task<Budget?> GetCurrentBudgetAsync(Guid companyId, Guid customerId)
        => dataContext.Budgets.FirstOrDefaultAsync(o => o.CustomerId == customerId && o.CompanyId == companyId);
}