namespace Curacaru.Backend.Controllers;

using System.Security.Authentication;
using System.Security.Claims;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    /// <summary>Gets the auth id.</summary>
    public string AuthId => User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    /// <summary>Gets the company id or null if user is not associated with a company.</summary>
    public Guid CompanyId
        => User.Claims.Any(o => o.Type == "CompanyId")
            ? Guid.Parse(User.Claims.First(o => o.Type == "CompanyId").Value)
            : throw new AuthenticationException("User is not associated with a company.");

    /// <summary>Gets a value indicating whether the user is a manager.</summary>
    public bool IsManager => User.Claims.Any(o => o.Type == "Manager");
}