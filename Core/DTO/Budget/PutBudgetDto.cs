namespace Curacaru.Backend.Core.DTO.Budget;

/// <summary>DTO to replace the current budgets of a customer.</summary>
public class PutBudgetDto
{
    /// <summary>Gets or sets the new amount of care benefit.</summary>
    public decimal CareBenefitAmount { get; set; }

    /// <summary>Gets or sets the id of the customer the budget belongs to.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the new current preventive care amount.</summary>
    public decimal PreventiveCareAmount { get; set; }

    /// <summary>Gets or sets the new current relief amount.</summary>
    public decimal ReliefAmount { get; set; }

    /// <summary>Gets or sets the new current self payment amount.</summary>
    public decimal SelfPayAmount { get; set; }

    /// <summary>Gets or sets the new monthly self payment raise.</summary>
    public decimal SelfPayRaise { get; set; }
}