namespace Curacaru.Backend.Core.DTO.Budget;

using Entities;

/// <summary>DTO to get the budget of a customer.</summary>
public class GetBudgetDto
{
    /// <inheritdoc cref="Budget.CareBenefitAmount" />
    public decimal CareBenefitAmount { get; set; }

    /// <summary>Gets or sets the value the care benefit is reset to every month.</summary>
    public decimal? CareBenefitRaise { get; set; }

    /// <inheritdoc cref="Customer.Id" />
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the full name of the customer.</summary>
    public string CustomerName { get; set; } = "";

    /// <inheritdoc cref="Customer.DoClearanceCareBenefit" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <inheritdoc cref="Customer.DoClearancePreventiveCare" />
    public bool DoClearancePreventiveCare { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceReliefAmount" />
    public bool DoClearanceReliefAmount { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceSelfPayment" />
    public bool DoClearanceSelfPayment { get; set; }

    /// <summary>Gets or sets the id of the budget.</summary>
    public Guid Id { get; set; }

    /// <inheritdoc cref="Budget.PreventiveCareAmount" />
    public decimal PreventiveCareAmount { get; set; }

    /// <summary>Gets or sets the value the preventive care amount is reset to every year.</summary>
    public decimal? PreventiveCareRaise { get; set; }

    /// <inheritdoc cref="Company.PricePerHour" />
    public decimal PricePerHour { get; set; }

    /// <inheritdoc cref="Budget.ReliefAmount" />
    public decimal ReliefAmount { get; set; }

    /// <summary>Gets or sets the relief amount from the current year.</summary>
    public decimal? ReliefAmountCurrentYear { get; set; }

    /// <summary>Gets or sets the remaining relief amount from the previous year.</summary>
    public decimal? ReliefAmountPreviousYear { get; set; }

    /// <summary>Gets or sets the value the  relief amount is raised every month.</summary>
    public decimal? ReliefAmountRaise { get; set; }

    /// <inheritdoc cref="Budget.SelfPayAmount" />
    public decimal SelfPayAmount { get; set; }

    /// <inheritdoc cref="Budget.SelfPayRaise" />
    public decimal? SelfPayRaise { get; set; }
}