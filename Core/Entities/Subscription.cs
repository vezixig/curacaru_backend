namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Subscription
{
    public Guid CompanyId { get; set; }

    [Key]
    public Guid Id { get; set; }

    public bool IsCanceled { get; set; }

    public DateTime? PeriodEnd { get; set; }

    public DateTime PeriodStart { get; set; }

    public string PriceId { get; set; } = "";

    public string StripeId { get; set; } = "";

    public string SubscriptionId { get; set; } = "";

    public SubscriptionType Type { get; set; } = SubscriptionType.Free;
}