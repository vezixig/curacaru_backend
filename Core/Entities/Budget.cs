namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

/// <summary>The current month's budget for a customer used for clearance.</summary>
public class Budget
{
    /// <summary>Gets or sets the current care benefit amount.</summary>
    /// <remarks>
    ///     the care benefit amount can only be used for the current month.
    ///     After the month is over the amount is reset to the amount granted according to the care level.
    /// </remarks>
    public decimal CareBenefitAmount { get; set; }

    /// <summary>Gets or sets the id of the company the budget belongs to.</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public required Customer Customer { get; set; }

    /// <summary>Gets or sets the id of the customer the budget belongs to.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the id of the budget.</summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets the month the budget is valid for.</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the current preventive care amount.</summary>
    /// <remarks>
    ///     The preventive care amount can be used for the current year.
    ///     Once the year is over the amount is reset.
    /// </remarks>
    public decimal PreventiveCareAmount { get; set; }

    /// <summary>Gets or sets the current relief amount.</summary>
    /// <remarks>
    ///     The relief amount can be charged until 30th june of the following year.
    ///     Every month the customer is granted additional 125€.
    /// </remarks>
    public decimal ReliefAmount { get; set; }

    /// <summary>Gets or sets the remaining relief amount of the last year.</summary>
    public decimal ReliefAmountLastYear { get; set; }

    /// <summary>Gets or sets the current self payment amount.</summary>
    public decimal SelfPayAmount { get; set; }

    /// <summary>Gets or sets the monthly self payment raise.</summary>
    public decimal SelfPayRaise { get; set; }

    /// <summary>Gets or sets the year the budget is valid for.</summary>
    public int Year { get; set; }
}