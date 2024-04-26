namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface ICompanyRepository
{
    /// <summary>Adds a company.</summary>
    /// <param name="company">The company to add.</param>
    /// <returns>The added company.</returns>
    public Task<Company> AddCompanyAsync(Company company);

    /// <summary>Gets a company by its id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <returns>The company or null if none is found.</returns>
    public Task<Company?> GetCompanyByIdAsync(Guid companyId);

    /// <summary>Updates an existing company.</summary>
    /// <param name="company">The modified company.</param>
    /// <returns>The updated company.</returns>
    public Task<Company> UpdateCompanyAsync(Company company);
}