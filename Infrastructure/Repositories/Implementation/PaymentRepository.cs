namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class PaymentRepository(DataContext context) : IPaymentRepository
{
    public Task AddSubscriptionAsync(Subscription subscription)
    {
        context.Subscriptions.Add(subscription);
        return context.SaveChangesAsync();
    }

    public Task DeleteSubscription(Guid companyId)
    {
        var subscriptions = context.Subscriptions.Where(o => o.CompanyId == companyId);
        context.Subscriptions.RemoveRange(subscriptions);
        return context.SaveChangesAsync();
    }

    public Task<List<Subscription>> GetActiveSubscriptions(Guid companyId)
        => context.Subscriptions.Where(o => o.CompanyId == companyId && (o.PeriodEnd == null || o.PeriodEnd > DateTime.UtcNow)).ToListAsync();

    public Task<List<Subscription>> GetSubscriptionsAsync(Guid companyId)
        => context.Subscriptions.Where(o => o.CompanyId == companyId).ToListAsync();

    public Task UpdateSubscriptionAsync(Subscription subscription)
    {
        context.Subscriptions.Update(subscription);
        return context.SaveChangesAsync();
    }
}