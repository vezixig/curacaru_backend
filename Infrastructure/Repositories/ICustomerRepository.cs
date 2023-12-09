namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

/// <summary>Repository for <see cref="Customer" />.</summary>
public interface ICustomerRepository
{
    /// <summary>Gets all customers of a company.</summary>
    /// <param name="companyId">The company identifier.</param>
    /// <returns>A list of customers.</returns>
    public Task<List<Customer>> GetCustomersAsync(Guid companyId);
}