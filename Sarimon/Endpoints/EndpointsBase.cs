namespace Curacaru.Backend.Endpoints;

using System.Security.Authentication;
using System.Security.Claims;

public class EndpointsBase
{
    public static string GetAuthId(ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    public static Guid GetCompanyId(ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.Any(o => o.Type == "CompanyId")
            ? Guid.Parse(claimsPrincipal.Claims.First(o => o.Type == "CompanyId").Value)
            : throw new AuthenticationException("User is not associated with a company.");
}