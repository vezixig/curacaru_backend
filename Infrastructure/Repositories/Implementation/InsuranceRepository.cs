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

    public Task<Insurance?> GetInsurance(Guid companyId, Guid insuranceId)
        => dataContext.Insurances
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == insuranceId);

    public Task<List<Insurance>> GetInsurances(Guid companyId)
        => dataContext.Insurances
            .Where(o => o.CompanyId == companyId)
            .ToListAsync();

    public Task<List<Insurance>> SearchInsurancesByNameAsync(Guid companyId, string name)
        => dataContext.Insurances
            .Where(o => o.CompanyId == companyId && o.Name.ToLower().Contains(name.ToLower()))
            .Take(5)
            .ToListAsync();
}