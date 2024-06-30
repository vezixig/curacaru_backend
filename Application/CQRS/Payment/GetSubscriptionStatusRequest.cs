namespace Curacaru.Backend.Application.CQRS.Payment;

using AutoMapper;
using Core.DTO.Payment;
using Core.Enums;
using Infrastructure.Repositories;
using MediatR;
using Services.Implementations;

public class GetSubscriptionStatusRequest(Guid companyId) : IRequest<GetSubscriptionDto>
{
    public Guid CompanyId { get; } = companyId;
}

internal class GetSubscriptionStatusRequestHandler(ICustomerRepository customerRepository, IMapper mapper, IPaymentRepository paymentRepository)
    : IRequestHandler<GetSubscriptionStatusRequest, GetSubscriptionDto>
{
    public async Task<GetSubscriptionDto> Handle(GetSubscriptionStatusRequest request, CancellationToken cancellationToken)
    {
        var subscriptions = await paymentRepository.GetActiveSubscriptions(request.CompanyId);
        var subscription = subscriptions.MinBy(o => o.PeriodStart);

        var result = subscription is null ? new() : mapper.Map<GetSubscriptionDto>(subscription);

        result.MaxCustomers = subscription is null ? 0 : SubscriptionService.GetMaxCustomers(subscription.Type);
        result.CurrentCustomers = await customerRepository.GetCustomerCountAsync(request.CompanyId, null, CustomerStatus.Customer);

        if (subscriptions.Count > 1)
        {
            var nextSubscription = subscriptions.MaxBy(o => o.PeriodStart);
            result.NextPeriodStart = nextSubscription!.PeriodStart;
            result.NextPeriodType = nextSubscription.Type;
        }

        return result;
    }
}