namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IInsuranceRepository
{
    /// <summary>Adds a new insurance to the database.</summary>
    /// <param name="insurance">The insurance to add.</param>
    /// <returns>The added insurance.</returns>
    public Task<Insurance> AddInsuranceAsync(Insurance insurance);

    /// <summary>Gets an insurance by id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="insuranceId">The insurance id.</param>
    /// <returns>The insurance or null if not found.</returns>
    public Task<Insurance?> GetInsurance(Guid companyId, Guid insuranceId);

    /// <summary>Gets all insurances of a company.</summary>
    /// <param name="companyId">The company id.</param>
    /// <returns>A list of insurances.</returns>
    public Task<List<Insurance>> GetInsurances(Guid companyId);

    /// <summary>Performs a search for insurances by name.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="name">The name or part of the name to search for.</param>
    /// <returns>A list of max. 5 search results.</returns>
    public Task<List<Insurance>> SearchInsurancesByNameAsync(Guid companyId, string name);
}