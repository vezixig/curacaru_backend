namespace Curacaru.Backend.Core.DTO.Budget;

/// <summary>DTO to get an entry for the budget list.</summary>
public class GetBudgetListEntryDto
{
    /// <summary>Gets or sets the id of the customer.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the name of the customer.</summary>
    public string CustomerName { get; set; } = "";

    /// <summary>Gets or sets the remaining care hours that can be afforded by the customer.</summary>
    public decimal RemainingHours { get; set; }

    /// <summary>Gets or sets the total amount of the budget.</summary>
    public decimal TotalAmount { get; set; }
}