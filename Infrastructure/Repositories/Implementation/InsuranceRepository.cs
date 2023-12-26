namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class InsuranceRepository(DataContext dataContext) : IInsuranceRepository
{
    public Task<List<Insurance>> GetInsurances(Guid companyId)
        => dataContext.Insurances
            .Where(insurance => insurance.CompanyId == companyId)
            .ToListAsync();

    public Task<List<Insurance>> SearchInsurancesByNameAsync(string name)
        => dataContext.Insurances
            .Where(insurance => insurance.Name.ToLower().Contains(name.ToLower()))
            .Take(5)
            .ToListAsync();
}