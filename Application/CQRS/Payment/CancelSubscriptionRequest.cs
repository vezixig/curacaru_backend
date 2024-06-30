namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class CancelSubscriptionRequest(Guid companyId) : IRequest
{
    public Guid CompanyId { get; } = companyId;
}

internal class CancelSubscriptionRequestHandler(IPaymentRepository paymentRepository, IStripeService stripeService) : IRequestHandler<CancelSubscriptionRequest>
{
    public async Task Handle(CancelSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscriptions = await paymentRepository.GetActiveSubscriptions(request.CompanyId);
        var subscription = subscriptions.Find(o => o.Type != SubscriptionType.Free)
                           ?? throw new NotFoundException("Subscription not found.");

        if (subscription.IsCanceled) throw new BadRequestException("Subscription is already canceled.");

        var stripeSubscription = await stripeService.CancelSubscription(subscription.SubscriptionId);

        subscription.IsCanceled = true;
        subscription.PeriodEnd = stripeSubscription.CurrentPeriodEnd;
        await paymentRepository.UpdateSubscriptionAsync(subscription);
    }
}