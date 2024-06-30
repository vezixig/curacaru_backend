namespace Curacaru.Backend.Core.DTO.Payment;

using Enums;

public record GetSubscriptionDto
{
    public int CurrentCustomers { get; set; }

    public bool IsActive { get; init; }

    public int MaxCustomers { get; set; }

    public DateTime? NextPeriodStart { get; set; }

    public SubscriptionType? NextPeriodType { get; set; }

    public DateTime? PeriodEnd { get; init; }

    public SubscriptionType Type { get; init; } = SubscriptionType.None;
}