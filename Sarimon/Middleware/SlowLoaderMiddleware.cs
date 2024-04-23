namespace Curacaru.Backend.Middleware;

/// <summary>Middleware to simulate a slow loading time.</summary>
public class SlowLoaderMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await Task.Delay(2000);
        await next(context);
    }
}