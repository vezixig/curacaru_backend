namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class InsuranceRepository : IInsuranceRepository
{
    private readonly DataContext _dataContext;

    public InsuranceRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public Task<List<Insurance>> SearchInsurancesByNameAsync(string name)
        => _dataContext.Insurances
            .Where(insurance => insurance.Name.ToLower().Contains(name.ToLower()))
            .Take(5)
            .ToListAsync();
}