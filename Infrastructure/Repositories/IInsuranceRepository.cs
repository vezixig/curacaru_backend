namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IInsuranceRepository
{
    /// <summary>Gets all insurances of a company.</summary>
    /// <param name="companyId">The company id.</param>
    /// <returns>A list of insurances.</returns>
    public Task<List<Insurance>> GetInsurances(Guid companyId);

    /// <summary>Performs a search for insurances by name.</summary>
    /// <param name="name">The name or part of the name to search for.</param>
    /// <returns>A list of max. 5 search results.</returns>
    public Task<List<Insurance>> SearchInsurancesByNameAsync(string name);
}