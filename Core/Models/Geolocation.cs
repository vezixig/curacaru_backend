namespace Curacaru.Backend.Core.Models;

/// <summary>Geolocation of an address.</summary>
/// <param name="Latitude">The latitude.</param>
/// <param name="Longitude">The longitude.</param>
public record Geolocation(decimal Latitude, decimal Longitude)
{
}