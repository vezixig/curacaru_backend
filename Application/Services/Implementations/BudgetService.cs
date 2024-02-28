namespace Curacaru.Backend.Application.Services.Implementations;

public class BudgetService
{
    /// <summary>Gets the monthly relief amount raise.</summary>
    public const decimal MonthlyReliefAmountRaise = 125;

    /// <summary>Gets the yearly preventive care raise.</summary>
    public const decimal YearlyPreventiveCareRaise = 1612;

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