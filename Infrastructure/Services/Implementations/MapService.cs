namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Text.Json;
using Core.Exceptions;
using Core.Models;

internal class MapService : IMapService
{
    public async Task<Geolocation> GetGeolocationAsync(string street, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentNullException(nameof(street));
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentNullException(nameof(zipCode));

        var queryParameter = $"{street.Replace(" ", "+")}+{zipCode}";
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Curacaru");

        var response = await httpClient.GetAsync($"https://nominatim.openstreetmap.org/search.php?q={queryParameter}&format=jsonv2");

        if (!response.IsSuccessStatusCode)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            throw new BadRequestException($"GEO Lokation konnte nicht abgerufen werden: {responseText}");
        }

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = jsonDocument.RootElement[0];
        var latitude = decimal.Parse(root.GetProperty("lat").GetString() ?? string.Empty);
        var longitude = decimal.Parse(root.GetProperty("lon").GetString() ?? string.Empty);
        return new Geolocation(latitude, longitude);
    }
}