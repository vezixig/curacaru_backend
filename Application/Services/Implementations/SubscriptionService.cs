namespace Curacaru.Backend.Application.Services.Implementations;

using Core.Enums;

public class SubscriptionService
{
    public static int GetMaxCustomers(SubscriptionType type)
        => type switch
        {
            SubscriptionType.Free => 0,
            SubscriptionType.Starter => 20,
            SubscriptionType.Business => 70,
            SubscriptionType.Pro => 200,
            SubscriptionType.Premium => 5000,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}