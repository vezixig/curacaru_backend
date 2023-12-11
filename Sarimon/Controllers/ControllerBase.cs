namespace Curacaru.Backend.Controllers;

using System.Security.Claims;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    public string AuthId => User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    public Guid? CompanyId => User.Claims.Any(o => o.Type == "CompanyId") ? Guid.Parse(User.Claims.First(o => o.Type == "CompanyId").Value) : null;

    public bool IsManager => User.Claims.Any(o => o.Type == "Manager");
}