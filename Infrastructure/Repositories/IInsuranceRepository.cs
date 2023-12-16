namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IInsuranceRepository
{
    /// <summary>Performs a search for insurances by name.</summary>
    /// <param name="name">The name or part of the name to search for.</param>
    /// <returns>A list of max. 5 search results.</returns>
    public Task<List<Insurance>> SearchInsurancesByNameAsync(string name);
}