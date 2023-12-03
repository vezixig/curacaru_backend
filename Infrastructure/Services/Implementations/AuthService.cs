namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

internal class AuthService : IAuthService
{
    private readonly IMemoryCache _cache;

    public AuthService(IMemoryCache cache)
        => _cache = cache;

    public async Task<string> GetMailAsync(string authId)
    {
        var token = await GetTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-7tgtetd5qydtostt.eu.auth0.com/api/v2/users/{authId}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var mail = jsonDocument.RootElement.GetProperty("email").GetString();
        return mail;
    }

    private async Task<string> GetTokenAsync()
    {
        if (_cache.TryGetValue("token", out string? cacheToken) && !string.IsNullOrEmpty(cacheToken)) return cacheToken;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-7tgtetd5qydtostt.eu.auth0.com/oauth/token");

        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", Environment.GetEnvironmentVariable("IDENTITY_CLIENTID") },
            { "client_secret", Environment.GetEnvironmentVariable("IDENTITY_SECRET") },
            { "audience", "https://dev-7tgtetd5qydtostt.eu.auth0.com/api/v2/" }
        };

        request.Content = new FormUrlEncodedContent(formData);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var token = jsonDocument.RootElement.GetProperty("access_token").GetString();

        _cache.Set("token", token, TimeSpan.FromMinutes(60));
        return token;
    }
}