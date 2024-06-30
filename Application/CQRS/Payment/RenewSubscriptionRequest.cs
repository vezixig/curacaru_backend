namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class RenewSubscriptionRequest(Guid companyId) : IRequest
{
    public Guid CompanyId { get; } = companyId;
}

internal class RenewSubscriptionRequestHandler(IPaymentRepository paymentRepository, IStripeService stripeService)
    : IRequestHandler<RenewSubscriptionRequest>
{
    public async Task Handle(RenewSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscriptions = await paymentRepository.GetActiveSubscriptions(request.CompanyId);
        var subscription = subscriptions.Find(o => o.Type != SubscriptionType.Free)
                           ?? throw new NotFoundException("Es gibt kein aktives Abonnement");

        await stripeService.RenewSubscription(subscription.SubscriptionId);

        subscription.IsCanceled = false;
        await paymentRepository.UpdateSubscriptionAsync(subscription);
    }
}