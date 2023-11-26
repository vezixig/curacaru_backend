namespace Curacaru.Backend.Infrastructure.repositories;

using Core.Entities;

public interface ICompanyRepository
{
    public Task<Company> AddCompanyAsync(Company company);
}