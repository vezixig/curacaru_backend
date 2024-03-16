namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class InsuranceRepository(DataContext dataContext) : IInsuranceRepository
{
    public async Task<Insurance> AddInsuranceAsync(Insurance insurance)
    {
        var dbInsurance = dataContext.Insurances.Add(insurance);
        await dataContext.SaveChangesAsync();
        return dbInsurance.Entity;
    }

    public Task DeleteInsuranceAsync(Insurance insurance)
    {
        dataContext.Insurances.Remove(insurance);
        return dataContext.SaveChangesAsync();
    }

    public Task<Insurance?> GetInsuranceAsync(Guid companyId, Guid insuranceId)
        => dataContext.Insurances
            .FirstOrDefaultAsync(o => (o.CompanyId == null || o.CompanyId == companyId) && o.Id == insuranceId);

    public Task<List<Insurance>> GetInsurancesAsync(Guid companyId)
        => dataContext.Insurances
            .Where(o => o.CompanyId == null || o.CompanyId == companyId)
            .Include(o => o.ZipCity)
            .OrderBy(o => o.CompanyId)
            .ThenBy(o => o.Name)
            .ToListAsync();

    public Task<List<Insurance>> SearchInsurancesByNameAsync(Guid companyId, string name)
        => dataContext.Insurances
            .Where(o => (o.CompanyId == null || o.CompanyId == companyId) && o.Name.ToLower().Contains(name.ToLower()))
            .Take(5)
            .ToListAsync();

    public async Task<Insurance> UpdateInsuranceAsync(Insurance insurance)
    {
        var dbInsurance = dataContext.Insurances.Update(insurance);
        await dataContext.SaveChangesAsync();
        return dbInsurance.Entity;
    }
}