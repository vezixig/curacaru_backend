namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class StartPaymentSessionRequest(Guid companyId, SubscriptionType subscriptionType) : IRequest<string>
{
    public Guid CompanyId { get; } = companyId;

    public SubscriptionType SubscriptionType { get; } = subscriptionType;
}

internal class StartPaymentSessionRequestHandler(IStripeService stripeService, IPaymentRepository paymentRepository)
    : IRequestHandler<StartPaymentSessionRequest, string>
{
    public async Task<string> Handle(StartPaymentSessionRequest request, CancellationToken cancellationToken)
    {
        var subscription = await paymentRepository.GetActiveSubscriptions(request.CompanyId);
        if (subscription.Exists(o => o.Type != SubscriptionType.Free)) throw new BadRequestException("Für den Dienst ist bereits ein Abonnement angelegt.");

        var priceId = stripeService.GetPriceId(request.SubscriptionType);

        var remainingTrialPeriodDays = 0;
        if (subscription.Count > 0)
        {
            remainingTrialPeriodDays = (int)Math.Ceiling((subscription.Max(o => o.PeriodEnd) - DateTime.UtcNow)!.Value.TotalDays);
            if (remainingTrialPeriodDays < 0) remainingTrialPeriodDays = 0;
        }

        var sessionUrl = await stripeService.GetSessionUri(request.CompanyId, priceId, remainingTrialPeriodDays);
        return sessionUrl;
    }
}