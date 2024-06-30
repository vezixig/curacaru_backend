namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IPaymentRepository
{
    public Task AddSubscriptionAsync(Subscription subscription);
    Task DeleteSubscription(Guid companyId);

    public Task<List<Subscription>> GetActiveSubscriptions(Guid companyId);

    public Task<List<Subscription>> GetSubscriptionsAsync(Guid companyId);

    Task UpdateSubscriptionAsync(Subscription subscription);
}