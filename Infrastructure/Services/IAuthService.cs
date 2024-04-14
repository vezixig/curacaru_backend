namespace Curacaru.Backend.Infrastructure.Services;

using Core.Models;

/// <summary>Service to access the Auth0 API.</summary>
public interface IAuthService
{
    /// <summary>Creates a new user in Auth0.</summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>The password and authId of the user.</returns>
    public Task<UserPassword> CreateUserAsync(string email);

    /// <summary>Deletes a user in Auth0.</summary>
    /// <param name="authId">The authId of the user.</param>
    /// <returns>An awaitable task object.</returns>
    public Task DeleteUserAsync(string authId);

    /// <summary>Gets the email of a user.</summary>
    /// <param name="authId">The authId of the user.</param>
    /// <returns>The email of the user.</returns>
    public Task<string> GetMailAsync(string authId);

    /// <summary>Sends a password reset mail to a user.</summary>
    public Task SendPasswordResetMail(string email);
}