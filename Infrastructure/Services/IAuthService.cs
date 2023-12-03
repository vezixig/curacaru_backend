namespace Curacaru.Backend.Infrastructure.Services;

public interface IAuthService
{
    public Task<string> GetMailAsync(string authId);
}