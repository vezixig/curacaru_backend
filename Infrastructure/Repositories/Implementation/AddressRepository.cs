namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Microsoft.EntityFrameworkCore;

internal class AddressRepository : IAddressRepository
{
    private readonly DataContext _dataContext;

    public AddressRepository(DataContext dataContext)
        => _dataContext = dataContext;

    public Task<string?> GetCityAsync(string zipCode)
        => _dataContext.ZipCities.Where(o => o.ZipCode == zipCode).Select(o => o.City).FirstOrDefaultAsync();
}