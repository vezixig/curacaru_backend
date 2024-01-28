namespace Curacaru.Backend.Infrastructure.Services;

using Core.Models;

/// <summary>Report to query the open street map API.</summary>
public interface IMapService
{
    /// <summary>Gets the geolocation of a given address.</summary>
    /// <param name="street">The street.</param>
    /// <param name="zipCode">The zip code.</param>
    /// <returns>Latitude and longitude as tuple.</returns>
    Task<Geolocation> GetGeolocationAsync(string street, string zipCode);
}