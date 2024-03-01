namespace Curacaru.Backend.Core.DTO.Customer;

using Entities;

/// <summary>DTO for getting a customer with the current budget.</summary>
public class GetCustomerBudgetDto
{
    /// <inheritdoc cref="Customer.AssociatedEmployeeId" />
    public Guid AssociatedEmployeeId { get; set; }

    /// <inheritdoc cref="Budget.PreventiveCareAmount" />
    public decimal CareBenefitAmount { get; set; }

    /// <inheritdoc cref="Customer.Id" />
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the name of the customer.</summary>
    public string CustomerName { get; init; } = "";

    /// <inheritdoc cref="Customer.DoClearanceCareBenefit" />
    public bool DoClearanceCareBenefit { get; set; }

    /// <inheritdoc cref="Customer.DoClearancePreventiveCare" />
    public bool DoClearancePreventiveCare { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceReliefAmount" />
    public bool DoClearanceReliefAmount { get; set; }

    /// <inheritdoc cref="Customer.DoClearanceSelfPayment" />
    public bool DoClearanceSelfPayment { get; set; }

    /// <inheritdoc cref="Budget.PreventiveCareAmount" />
    public decimal PreventiveCareAmount { get; set; }

    /// <inheritdoc cref="Budget.ReliefAmount" />
    public decimal ReliefAmount { get; set; }

    /// <inheritdoc cref="Budget.SelfPayAmount" />
    public decimal SelfPayAmount { get; set; }
}