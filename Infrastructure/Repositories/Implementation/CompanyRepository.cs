namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class CompanyRepository(DataContext dataContext) : ICompanyRepository
{
    public async Task<Company> AddCompanyAsync(Company company)
    {
        var dbCompany = dataContext.Companies.Add(company);
        await dataContext.SaveChangesAsync();
        return dbCompany.Entity;
    }

    public Task<Company?> GetCompanyByIdAsync(Guid companyId)
        => dataContext.Companies
            .Include(o => o.ZipCity)
            .FirstOrDefaultAsync(c => c.Id == companyId);

    public async Task<Company> UpdateCompanyAsync(Company company)
    {
        var dbCompany = dataContext.Companies.Update(company);
        await dataContext.SaveChangesAsync();
        return dbCompany.Entity;
    }
}