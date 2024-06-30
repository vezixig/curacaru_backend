namespace Curacaru.Backend.Application.CQRS.Payment;

using Infrastructure.Repositories;
using MediatR;

public class DeleteSubscriptionRequest(Guid companyId) : IRequest
{
    public Guid CompanyId { get; } = companyId;
}

internal class DeleteSubscriptionRequestHandler(IPaymentRepository paymentRepository) : IRequestHandler<DeleteSubscriptionRequest>
{
    public async Task Handle(DeleteSubscriptionRequest request, CancellationToken cancellationToken)
    {
        await paymentRepository.DeleteSubscription(request.CompanyId);
    }
}