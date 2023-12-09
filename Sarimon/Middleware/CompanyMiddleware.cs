namespace Curacaru.Backend.Middleware;

using System.Security.Claims;
using Infrastructure.repositories;

public class CompanyMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var employeeRepository = context.RequestServices.GetService<IEmployeeRepository>() ?? throw new Exception("Employee repository is not set");

        // Check if the user is authenticated
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // Get the user's identity
            if (context.User.Identity is not ClaimsIdentity identity) throw new Exception("User is authenticated but identity is not set");

            // Retrieve user's ID from the claims
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var employee = await employeeRepository.GetEmployeeByAuthIdAsync(userIdClaim.Value);
                if (employee != null)
                {
                    if (employee.CompanyId != null) identity.AddClaim(new Claim("CompanyId", employee.CompanyId.ToString()));
                    if (employee.IsManager) identity.AddClaim(new Claim("Manager", ""));
                }
            }
        }

        await next(context);
    }
}