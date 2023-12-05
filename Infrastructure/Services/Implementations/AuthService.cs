namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Core.Models;
using Microsoft.Extensions.Caching.Memory;

internal class AuthService : IAuthService
{
    private readonly string _baseUrl;

    private readonly IMemoryCache _cache;

    public AuthService(IMemoryCache cache)
    {
        _cache = cache;
        _baseUrl = Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY") + "api/v2/";
    }

    public async Task<UserPassword> CreateUserAsync(string email)
    {
        var token = await GetTokenAsync();
        var user = new
        {
            email,
            password = GeneratePassword(),
            verify_email = false,
            connection = "Username-Password-Authentication"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}users");
        request.Content = new StringContent(JsonSerializer.Serialize(user));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode) throw new Exception("Benutzer konnte nicht angelegt werden.");

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var userId = jsonDocument.RootElement.GetProperty("user_id").GetString();
        return new UserPassword(user.password, userId);
    }

    public async Task<string> GetMailAsync(string authId)
    {
        var token = await GetTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}users/{authId}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var client = new HttpClient();
        var response = await client.SendAsync(request);

        var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var mail = jsonDocument.RootElement.GetProperty("email").GetString();
        return mail;
    }

    private static string GeneratePassword()
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var password = new StringBuilder();
        var random = new Random();

        password.Append(validChars[random.Next(0, 26)]);
        password.Append(validChars[random.Next(26, 52)]);
        password.Append(validChars[random.Next(52, 62)]);

        for (var i = 0; i < 5; i++)
        {
            var index = random.Next(validChars.Length);
            password.Append(validChars[index]);
        }

        return password.ToString();
    }

    private async Task<string> GetTokenAsync()
    {
        if (_cache.TryGetValue("token", out string? cacheToken) && !string.IsNullOrEmpty(cacheToken)) return cacheToken;

        var request = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY") + "oauth/token");

        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", Environment.GetEnvironmentVariable("IDENTITY_CLIENTID") },
            { "client_secret", Environment.GetEnvironmentVariable("IDENTITY_SECRET") },
            { "audience", _baseUrl }
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