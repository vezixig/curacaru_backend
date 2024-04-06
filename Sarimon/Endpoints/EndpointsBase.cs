namespace Curacaru.Backend.Endpoints;

using System.Security.Authentication;
using System.Security.Claims;
using Core;

/// <summary>Base class for all endpoints classes.</summary>
public class EndpointsBase
{
    /// <summary>Gets the authenticated user's id.</summary>
    public string GetAuthId(ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    /// <summary>Gets the authenticated user's company id.</summary>
    /// <exception cref="AuthenticationException">If no CompanyId claim is found in the principal.</exception>
    public Guid GetCompanyId(ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.Any(o => o.Type == "CompanyId")
            ? Guid.Parse(claimsPrincipal.Claims.First(o => o.Type == "CompanyId").Value)
            : throw new AuthenticationException("User is not associated with a company.");

    /// <summary>Gets the user id.</summary>
    public Guid GetUserId(ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.Any(o => o.Type == Constants.Claims.UserId)
            ? Guid.Parse(claimsPrincipal.Claims.First(o => o.Type == Constants.Claims.UserId).Value)
            : throw new AuthenticationException("User does not exist.");
}