namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class DowngradeSubscriptionRequest(Guid companyId, SubscriptionType subscriptionType) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public SubscriptionType SubscriptionType { get; } = subscriptionType;
}

internal class DowngradeSubscriptionRequestHandler(IPaymentRepository paymentRepository, IStripeService stripeService)
    : IRequestHandler<DowngradeSubscriptionRequest>
{
    public async Task Handle(DowngradeSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscriptions = await paymentRepository.GetActiveSubscriptions(request.CompanyId);

        var subscription = subscriptions.Find(o => o.Type != SubscriptionType.Free)
                           ?? throw new NotFoundException("Subscription not found.");

        if (subscriptions.Count > 1 || subscription.Type <= request.SubscriptionType) throw new BadRequestException("Dein Abo kann kein Downgrade erhalten.");

        var priceId = stripeService.GetPriceId(request.SubscriptionType);

        var nextStripeSubscription = await stripeService.DowngradeSubscription(subscription.SubscriptionId, priceId);

        var nextSubscription = new Subscription
        {
            StripeId = subscription.StripeId,
            CompanyId = request.CompanyId,
            Type = request.SubscriptionType,
            PeriodStart = nextStripeSubscription.StartDate
        };
        await paymentRepository.AddSubscriptionAsync(nextSubscription);
    }
}