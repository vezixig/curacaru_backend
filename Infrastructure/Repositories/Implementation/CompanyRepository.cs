namespace Curacaru.Backend.Infrastructure.repositories.implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

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

    public Task<Company?> GetCompanyById(Guid employeCompanyId)
        => _dataContext.Companies.FirstOrDefaultAsync(c => c.Id == employeCompanyId);
}