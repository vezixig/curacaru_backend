namespace Curacaru.Backend.Infrastructure.Services;

using Core.Models;

public interface IAuthService
{
    public Task<UserPassword> CreateUserAsync(string email);
    public Task<string> GetMailAsync(string authId);
}