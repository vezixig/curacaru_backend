namespace Curacaru.Backend.Controllers;

using System.Security.Claims;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    public string AuthId => User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
}