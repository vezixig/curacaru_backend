namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class AddressRepository(DataContext dataContext) : IAddressRepository
{
    public Task<string?> GetCityAsync(string zipCode)
        => dataContext.ZipCities.Where(o => o.ZipCode == zipCode).Select(o => o.City).FirstOrDefaultAsync();

    public Task<ZipCity?> GetZipCityAsync(string zipCode)
        => dataContext.ZipCities.AsTracking().FirstOrDefaultAsync(o => o.ZipCode == zipCode);
}