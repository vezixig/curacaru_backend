namespace BudgetReplenisher;

using Curacaru.Backend.Application.Services;
using Curacaru.Backend.Application.Services.Implementations;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Core.Enums;
using Curacaru.Backend.Core.Models;
using Curacaru.Backend.Infrastructure.Repositories;
using Curacaru.Backend.Infrastructure.Services;

public class Worker(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    IBudgetService budgetService,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService)
{
    public async Task DoWorkAsync()
    {
        await ProcessBudgets();
        await ProcessAppointments();
    }

    private async Task ProcessAppointments()
    {
        var appointments = await appointmentRepository.GetPlannedAppointmentsOfCurrentMonthAsync();

        foreach (var appointment in appointments)
        {
            appointment.Costs = await budgetService.CalculateAppointmentPriceAsync(PriceCalculationData.CreateFrom(appointment));
            appointment.IsPlanned = false;

            var budget = await budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, appointment.CustomerId);
            if (budget == null)
            {
                appointment.HasBudgetError = true;
                continue;
            }

            switch (appointment.ClearanceType)
            {
                case ClearanceType.SelfPayment:
                    if (budget.SelfPayAmount < appointment.Costs)
                        appointment.HasBudgetError = true;
                    else
                        budget.SelfPayAmount -= appointment.Costs;
                    break;
                case ClearanceType.CareBenefit:
                    if (budget.CareBenefitAmount < appointment.Costs)
                        appointment.HasBudgetError = true;
                    else
                        budget.CareBenefitAmount -= appointment.Costs;
                    break;
                case ClearanceType.PreventiveCare:
                    if (budget.PreventiveCareAmount < appointment.Costs)
                        appointment.HasBudgetError = true;
                    else
                        budget.PreventiveCareAmount -= appointment.Costs;
                    break;
                case ClearanceType.ReliefAmount:
                    if (budget.ReliefAmount + budget.ReliefAmountLastYear < appointment.Costs)
                    {
                        appointment.HasBudgetError = true;
                        break;
                    }

                    budget.ReliefAmountLastYear -= appointment.Costs;
                    appointment.CostsLastYearBudget = appointment.Costs;

                    if (budget.ReliefAmountLastYear < 0)
                    {
                        budget.ReliefAmount += budget.ReliefAmountLastYear;
                        appointment.CostsLastYearBudget += budget.ReliefAmountLastYear;
                        appointment.Costs += budget.ReliefAmountLastYear * -1;
                        budget.ReliefAmountLastYear = 0;
                    }

                    break;
            }

            var transaction = await databaseService.BeginTransactionAsync(CancellationToken.None);
            try
            {
                if (appointment.HasBudgetError)
                {
                    appointment.Costs = 0;
                    appointment.CostsLastYearBudget = 0;
                }

                await appointmentRepository.UpdateAppointmentAsync(appointment);
                await budgetRepository.UpdateBudgetAsync(budget);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private async Task ProcessBudgets()
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