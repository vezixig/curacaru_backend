namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;
using Core.Enums;

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
    /// <remarks>Is used in the service.</remarks>
    public Task<List<Customer>> GetAllCustomersAsync();

    /// <summary>Gets a customer.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="customerId">The employee id.</param>
    /// <param name="employeeId">An optional employee id to check access.</param>
    /// <param name="asTracking">True to track the entity.</param>
    /// <returns>A customer if found, otherwise null.</returns>
    public Task<Customer?> GetCustomerAsync(
        Guid companyId,
        Guid customerId,
        Guid? employeeId = null,
        bool asTracking = false);

    /// <summary>Gets the count of customers.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="employeeId">An optional employee id to check access.</param>
    /// <param name="status">The status to filter customers by.</param>
    /// <returns>The count of customers.</returns>
    Task<int> GetCustomerCountAsync(Guid companyId, Guid? employeeId, CustomerStatus status);

    /// <summary>Gets all customers of a company.</summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="employeeId">An optional employee id to check access.</param>
    /// <param name="insuranceStatus">An optional filter for the insurance status.</param>
    /// <param name="requestAssignmentDeclarationYear">
    ///     An optional filter to remove customers that already have an assignment
    ///     declaration for the provided year.
    /// </param>
    /// <param name="customerId">If set only the customer with the id is returned.</param>
    /// <param name="status">An optional filter for the customer status.</param>
    /// <returns>A list of customers.</returns>
    public Task<List<Customer>> GetCustomersAsync(
        Guid companyId,
        Guid? employeeId = null,
        InsuranceStatus? insuranceStatus = null,
        int? requestAssignmentDeclarationYear = null,
        Guid? customerId = null,
        CustomerStatus? status = null,
        int? page = null,
        int? pageSize = null);

    /// <summary>
    ///     Gets all customers of a company that are assigned to a specific employee or have a replacement appointment or
    ///     an existing deployment report.
    /// </summary>
    Task<List<Customer>> GetCustomersForDeploymentReportsAsync(
        Guid companyId,
        Guid? employeeId);

    /// <summary>Updates a customer.</summary>
    /// <param name="customer">The modified customer.</param>
    /// <returns>The updated customer.</returns>
    public Task<Customer> UpdateCustomerAsync(Customer customer);
}