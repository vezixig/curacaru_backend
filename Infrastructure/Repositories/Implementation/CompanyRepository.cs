namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;

internal class CompanyRepository : ICompanyRepository
{
    private readonly DataContext _dataContext;

    public CompanyRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public async Task<Company> AddCompanyAsync(Company company)
    {
        var dbCompany = _dataContext.Companies.Add(company);
        await _dataContext.SaveChangesAsync();
        return dbCompany.Entity;
    }
}