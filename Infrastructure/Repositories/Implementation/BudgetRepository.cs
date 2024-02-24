namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class BudgetRepository(DataContext dataContext) : IBudgetRepository
{
    public Task AddBudgetAsync(Budget budget)
    {
        dataContext.Attach(budget.Customer);
        dataContext.Budgets.Add(budget);
        return dataContext.SaveChangesAsync();
    }

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
        => dataContext.Budgets.FirstOrDefaultAsync(
            o => o.CustomerId == customerId && o.CompanyId == companyId && o.Year == DateTime.Today.Year && o.Month == DateTime.Today.Month);

    public Task<Budget?> GetLastYearBudgetAsync(Guid companyId, Guid customerId)
        => dataContext.Budgets.FirstOrDefaultAsync(
            o => o.CustomerId == customerId && o.CompanyId == companyId && o.Year == DateTime.Today.Year - 1 && o.Month == 12);

    public Task UpdateBudgetAsync(Budget budget)
    {
        dataContext.Budgets.Update(budget);
        return dataContext.SaveChangesAsync();
    }
}