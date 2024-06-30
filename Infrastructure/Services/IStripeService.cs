namespace Curacaru.Backend.Infrastructure.Services;

using Core.Enums;
using Stripe;
using Stripe.Checkout;

/// <summary>Service to interact with Stripe.</summary>
public interface IStripeService
{
    Task<Subscription> CancelSubscription(string subscriptionId);

    Task<Subscription> DowngradeSubscription(string subscriptionId, string priceId);
    string GetPriceId(SubscriptionType requestSubscriptionType);
    Task<Session> GetSession(string sessionId);

    /// <summary>Gets a URI for the payment session.</summary>
    Task<string> GetSessionUri(Guid companyId, string priceId, int trialPeriodDays);

    Task<Subscription> GetSubscription(string subscriptionId);
    SubscriptionType GetSubscriptionTypeFromPriceId(string priceId);

    Task<Subscription> RenewSubscription(string subscriptionId);

    Task<Subscription> UpgradeSubscription(string subscriptionSubscriptionId, string priceId);
}