namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using Core.Enums;
using Stripe;
using Stripe.Checkout;

internal class StripeService : IStripeService
{
    private readonly StripeClient _client = new(Environment.GetEnvironmentVariable("STRIPE_API_KEY"));

    private readonly SessionService _service;

    private readonly SubscriptionScheduleService _subscriptionScheduleService;

    private readonly SubscriptionService _subscriptionService;

    public StripeService()
    {
        _service = new(_client);
        _subscriptionService = new(_client);
        _subscriptionScheduleService = new(_client);
    }

    public Task<Subscription> CancelSubscription(string subscriptionId)
        => _subscriptionService.UpdateAsync(subscriptionId, new() { CancelAtPeriodEnd = true });

    public async Task<Subscription> DowngradeSubscription(string subscriptionId, string priceId)
    {
        var subscription = await _subscriptionService.GetAsync(subscriptionId);

        // Calculate the end date of the current period
        var currentPeriodEnd = subscription.CurrentPeriodEnd;

        // Create a subscription schedule with the future phase
        var scheduleOptions = new SubscriptionScheduleCreateOptions
        {
            Customer = subscription.CustomerId,
            StartDate = currentPeriodEnd,
            EndBehavior = "release",
            Phases =
            [
                new()
                {
                    Items = [new() { Price = priceId, Quantity = 1 }]
                }
            ]
        };
        var schedule = await _subscriptionScheduleService.CreateAsync(scheduleOptions);

        subscription.CancelAtPeriodEnd = true;
        await _subscriptionService.UpdateAsync(subscriptionId, new() { CancelAtPeriodEnd = true });

        return new()
        {
            StartDate = schedule.Phases[0].StartDate
        };
    }

    public string GetPriceId(SubscriptionType requestSubscriptionType)
    {
        return requestSubscriptionType switch
        {
            SubscriptionType.Starter => "price_1PTQ1mIk8tH2HUNW9iACICCe",
            SubscriptionType.Business => "price_1PUZfzIk8tH2HUNW8sZyjH4C",
            SubscriptionType.Pro => "price_1PWMmCIk8tH2HUNW8AomqRtu",
            SubscriptionType.Premium => "price_1PWMn4Ik8tH2HUNWN3XIE3h6",
            _ => throw new ArgumentOutOfRangeException(nameof(requestSubscriptionType), requestSubscriptionType, null)
        };
    }

    public async Task<Session> GetSession(string sessionId)
    {
        var session = await _service.GetAsync(sessionId);
        return session;
    }

    public async Task<string> GetSessionUri(Guid companyId, string priceId, int trialPeriodDays)
    {
        var options = new SessionCreateOptions
        {
            SuccessUrl = Environment.GetEnvironmentVariable("STRIPE_CALLBACK") + "?success&session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = Environment.GetEnvironmentVariable("STRIPE_CALLBACK") + "?canceled",
            ClientReferenceId = companyId.ToString(),
            Mode = "subscription",
            LineItems =
            [
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            ]
            // AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
        };

        if (trialPeriodDays > 0) options.SubscriptionData = new() { TrialPeriodDays = trialPeriodDays };

        var session = await _service.CreateAsync(options);
        return session.Url;
    }

    public async Task<Subscription> GetSubscription(string subscriptionId)
    {
        var subscription = await _subscriptionService.GetAsync(subscriptionId);
        return subscription;
    }

    public SubscriptionType GetSubscriptionTypeFromPriceId(string priceId)
        => priceId switch
        {
            "price_1PTQ1mIk8tH2HUNW9iACICCe" => SubscriptionType.Starter,
            "price_1PUZfzIk8tH2HUNW8sZyjH4C" => SubscriptionType.Business,
            "price_1PWMmCIk8tH2HUNW8AomqRtu" => SubscriptionType.Pro,
            "price_1PWMn4Ik8tH2HUNWN3XIE3h6" => SubscriptionType.Premium,
            _ => throw new ArgumentOutOfRangeException(nameof(priceId), priceId, null)
        };

    public Task<Subscription> RenewSubscription(string subscriptionId)
        => _subscriptionService.UpdateAsync(subscriptionId, new() { CancelAtPeriodEnd = false });

    public async Task<Subscription> UpgradeSubscription(string subscriptionId, string priceId)
    {
        var subscription = await _subscriptionService.GetAsync(subscriptionId);

        var result = await _subscriptionService.UpdateAsync(
            subscriptionId,
            new()
            {
                ProrationBehavior = "always_invoice",
                Items =
                [
                    new()
                    {
                        Id = subscription.Items.First().Id,
                        Price = priceId
                    }
                ]
            }
        );
        return result;
    }
}