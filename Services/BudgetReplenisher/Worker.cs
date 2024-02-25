namespace BudgetReplenisher;

using Curacaru.Backend.Application.Services;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Infrastructure.Repositories;

public class Worker(ICustomerRepository customerRepository, IBudgetRepository budgetRepository)
{
    public async Task DoWorkAsync()
    {
        var customers = await customerRepository.GetAllCustomersAsync();
        var budgets = await budgetRepository.GetAllBudgetsAsync();

        var month = DateTime.Now.Month;
        var year = DateTime.Now.Year;

        foreach (var customer in customers)
        {
            // add missing empty budgets
            var customerBudget = budgets.Find(o => o.CustomerId == customer.Id);
            if (customerBudget == null)
            {
                var budget = new Budget
                {
                    CustomerId = customer.Id,
                    CareBenefitAmount = 0,
                    PreventiveCareAmount = 0,
                    ReliefAmount = 0,
                    SelfPayAmount = 0,
                    SelfPayRaise = 0,
                    Customer = customer,
                    CompanyId = customer.CompanyId,
                    Month = month,
                    Year = year
                };
                await budgetRepository.AddBudgetAsync(budget);
                continue;
            }

            if (customerBudget.Year == year && customerBudget.Month == month) continue;

            // update yearly budgets
            if (customerBudget.Year != year)
            {
                customerBudget.Year = year;
                customerBudget.ReliefAmountLastYear = customer.DoClearanceReliefAmount ? customerBudget.ReliefAmount : 0;
                customerBudget.ReliefAmount = 0;
                customerBudget.PreventiveCareAmount = customer.DoClearancePreventiveCare ? BudgetService.YearlyPreventiveCareRaise : 0;
            }

            // update monthly budgets
            if (customerBudget.Month != month)
            {
                if (month > 6) customerBudget.ReliefAmountLastYear = 0;

                customerBudget.Month = month;
                customerBudget.ReliefAmount += customer.DoClearanceReliefAmount ? BudgetService.MonthlyReliefAmountRaise : 0;
                customerBudget.SelfPayAmount = customer.DoClearanceSelfPayment ? customerBudget.SelfPayRaise : 0;
                customerBudget.CareBenefitAmount = customer.DoClearanceCareBenefit ? BudgetService.GetCareBenefitByCareLevel(customer.CareLevel) : 0;
            }

            await budgetRepository.UpdateBudgetAsync(customerBudget);
        }
    }
}