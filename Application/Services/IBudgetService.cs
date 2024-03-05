namespace Curacaru.Backend.Application.Services;

using Core.Entities;
using Core.Models;

public interface IBudgetService
{
    /// <summary>Calculates the price of an appointment.</summary>
    /// <param name="appointment">The appointment to calculate the price for.</param>
    /// <returns>The price of the appointment.</returns>
    Task<decimal> CalculateAppointmentPriceAsync(PriceCalculationData appointment);

    /// <summary>Refunds the costs of an appointment.</summary>
    /// <param name="appointment">the appointment which costs should be refunded.</param>
    /// <returns>An awaitable task object.</returns>
    Task RefundBudget(Appointment appointment);
}