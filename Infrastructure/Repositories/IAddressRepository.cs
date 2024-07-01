namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IAddressRepository
{
    Task<string?> GetCityAsync(string zipCode);

    /// <summary>Gets the city of a zip code.</summary>
    /// <param name="zipCode">The zip code.</param>
    /// <returns>The city or null if none was found.</returns>
    Task<ZipCity?> GetZipCityAsync(string zipCode);
}