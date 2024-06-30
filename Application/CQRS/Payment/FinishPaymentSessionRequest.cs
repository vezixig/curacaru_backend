namespace Curacaru.Backend.Application.CQRS.Payment;

using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class FinishPaymentSessionRequest(Guid companyId, string sessionId) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public string SessionId { get; } = sessionId;
}

internal class FinishPaymentSessionRequestHandler(ICompanyRepository companyRepository, IStripeService stripeService, IPaymentRepository paymentRepository)
    : IRequestHandler<FinishPaymentSessionRequest>
{
    public async Task Handle(FinishPaymentSessionRequest request, CancellationToken cancellationToken)
    {
        var session = await stripeService.GetSession(request.SessionId);
        var currentSubscription = await paymentRepository.GetActiveSubscriptions(request.CompanyId);

        if (currentSubscription.Exists(o => o.Type != SubscriptionType.Free)) throw new BadRequestException("Ein Abonnement wurde bereits aktiviert.");

        if (session is { PaymentStatus: "paid", Status: "complete" })
        {
            var stripeSubscription = await stripeService.GetSubscription(session.SubscriptionId);

            var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);
            var subscription = new Subscription
            {
                CompanyId = company!.Id,
                StripeId = session.CustomerId,
                SubscriptionId = session.SubscriptionId,
                PeriodStart = stripeSubscription.CurrentPeriodStart,
                Type = stripeService.GetSubscriptionTypeFromPriceId(stripeSubscription.Items.First().Price.Id)
            };

            await paymentRepository.AddSubscriptionAsync(subscription);

            if (currentSubscription.Count > 0)
            {
                currentSubscription[0].IsCanceled = true;
                currentSubscription[0].PeriodEnd = DateTime.UtcNow;
                await paymentRepository.UpdateSubscriptionAsync(currentSubscription[0]);
            }
        }
    }
}