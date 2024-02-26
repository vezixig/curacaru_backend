namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

/// <summary>Repository for <see cref="Customer" />.</summary>
public interface ICustomerRepository
{
    /// <summary>Adds a new customer.</summary>
    /// <param name="customer">The new customer.</param>
    /// <returns>The added customer.</returns>
    public Task<Customer> AddCustomerAsync(Customer customer);

    /// <summary>Deletes a customer.</summary>
    /// <param name="customer">The customer to delete.</param>
    /// <returns>An awaitable task object.</returns>
    public Task DeleteCustomerAsync(Customer customer);

    /// <summary>Gets all customers.</summary>
    /// <returns>A list of customers.</returns>
    public Task<List<Customer>> GetAllCustomersAsync();

    /// <summary>Gets a customer.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="customerId">The employee id.</param>
    /// <param name="employeeId">An optional employee id to check access.</param>
    /// <returns>A customer if found, otherwise null.</returns>
    public Task<Customer?> GetCustomerAsync(Guid companyId, Guid customerId, Guid? employeeId = null);

    /// <summary>Gets all customers of a company.</summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="employeeId">An optional employee id to check access.</param>
    /// <returns>A list of customers.</returns>
    public Task<List<Customer>> GetCustomersAsync(Guid companyId, Guid? employeeId = null);

    /// <summary>Updates a customer.</summary>
    /// <param name="customer">The modified customer.</param>
    /// <returns>The updated customer.</returns>
    public Task<Customer> UpdateCustomerAsync(Customer customer);
}