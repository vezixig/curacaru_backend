namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class UpgradeSubscriptionRequest(Guid companyId, SubscriptionType type) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public SubscriptionType Type { get; } = type;
}

internal class UpgradeSubscriptionRequestHandler(IPaymentRepository paymentRepository, IStripeService stripeService)
    : IRequestHandler<UpgradeSubscriptionRequest>
{
    public async Task Handle(UpgradeSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscriptions = await paymentRepository.GetActiveSubscriptions(request.CompanyId);
        var subscription = subscriptions.Find(o => o.Type != SubscriptionType.Free) ?? throw new NotFoundException("Subscription not found.");

        if (subscription.Type >= request.Type) throw new BadRequestException("Dein Abo kann kein Upgrade erhalten.");

        var priceId = stripeService.GetPriceId(request.Type);
        await stripeService.UpgradeSubscription(subscription.SubscriptionId, priceId);

        subscription.Type = request.Type;
        await paymentRepository.UpdateSubscriptionAsync(subscription);
    }
}