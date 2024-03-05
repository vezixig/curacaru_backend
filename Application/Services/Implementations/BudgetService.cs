namespace Curacaru.Backend.Application.Services.Implementations;

using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.repositories;
using Infrastructure.Repositories;

public class BudgetService(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    ICompanyRepository companyRepository,
    IDateTimeService dateTimeService) : IBudgetService
{
    /// <summary>Gets the monthly relief amount raise.</summary>
    public const decimal MonthlyReliefAmountRaise = 125;

    /// <summary>Gets the yearly preventive care raise.</summary>
    public const decimal YearlyPreventiveCareRaise = 1612;

    public async Task<decimal> CalculateAppointmentPriceAsync(PriceCalculationData appointment)
    {
        var company = await companyRepository.GetCompanyByIdAsync(appointment.CompanyId);
        var ridePrice = company!.RideCostsType switch
        {
            RideCostsType.FlatRate => company.RideCosts,
            RideCostsType.Kilometer => company.RideCosts * appointment.DistanceToCustomer,
            _ => 0
        };
        var appointmentTime = appointment.TimeEnd - appointment.TimeStart;
        return company.PricePerHour * appointmentTime.Hours + ridePrice;
    }

    public async Task RefundBudget(Appointment appointment)
    {
        var budget = await budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, appointment.CustomerId)
                     ?? throw new BadRequestException("Budget nicht gefunden.");

        switch (appointment.ClearanceType)
        {
            case ClearanceType.SelfPayment:
                if (appointment.Date.Year != dateTimeService.Now.Year || appointment.Date.Month != dateTimeService.Now.Month) return;
                budget.SelfPayAmount += appointment.Costs;
                break;
            case ClearanceType.CareBenefit:
                if (appointment.Date.Year != dateTimeService.Now.Year || appointment.Date.Month != dateTimeService.Now.Month) return;
                budget.CareBenefitAmount += appointment.Costs;
                break;
            case ClearanceType.PreventiveCare:
                if (appointment.Date.Year != dateTimeService.Now.Year) return;
                budget.PreventiveCareAmount += appointment.Costs;
                break;
            case ClearanceType.ReliefAmount:
                // Check if budget from previous year is expired
                if (appointment.Date.Year < dateTimeService.Now.Year - 1 || (appointment.Date.Year < dateTimeService.Now.Year && dateTimeService.Now.Month > 6))
                    return;

                // If appointment is from previous year switch budget to process
                if (appointment.Date.Year < dateTimeService.Now.Year)
                {
                    appointment.CostsLastYearBudget = appointment.Costs;
                    appointment.Costs = 0;
                }

                // If current month is after july last year budget is expired
                if (dateTimeService.Now.Month > 6) appointment.CostsLastYearBudget = 0;

                var customerAppointments = await appointmentRepository.GetAppointmentsAsync(
                    appointment.CompanyId,
                    new DateOnly(dateTimeService.Now.Year, 1, 1),
                    new DateOnly(dateTimeService.Now.Year, 12, 31),
                    null,
                    appointment.CustomerId);

                // remove deleted appointment and all appointments that can not be booked on last year budget
                customerAppointments.RemoveAll(o => o.Id == appointment.Id || o.Costs == 0 || o.ClearanceType != ClearanceType.ReliefAmount);

                var appointmentIndex = 0;

                // refund budget from previous year
                while (customerAppointments.Count > 0 && appointment.CostsLastYearBudget > 0 && appointmentIndex < customerAppointments.Count)
                {
                    var refundAppointment = customerAppointments[appointmentIndex];
                    var refunded = Math.Min(refundAppointment.Costs, appointment.CostsLastYearBudget);

                    refundAppointment.CostsLastYearBudget += refunded;
                    refundAppointment.Costs -= refunded;
                    appointment.CostsLastYearBudget -= refunded;

                    await appointmentRepository.UpdateAppointmentAsync(refundAppointment);
                    appointmentIndex++;
                }

                budget.ReliefAmount += appointment.Costs;
                budget.ReliefAmountLastYear += appointment.CostsLastYearBudget;

                break;
        }

        await budgetRepository.UpdateBudgetAsync(budget);
    }

    /// <summary>Gets the monthly care benefit by care level.</summary>
    /// <param name="careLevel">The care level.</param>
    /// <returns>The monthly care benefit.</returns>
    public static decimal GetCareBenefitByCareLevel(int careLevel)
        => careLevel switch
        {
            2 => 304.4m,
            3 => 572.8m,
            4 => 711.2m,
            5 => 880.0m,
            _ => 0
        };
}