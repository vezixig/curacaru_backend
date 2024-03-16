namespace Curacaru.Backend.Service;

using Endpoints;

/// <summary>Registers all endpoints from the assembly</summary>
public static class RegisterEndpoints
{
    /// <summary>Scans the assembly for classes implementing <see cref="IEndpoints" /> and registers them at the app.</summary>
    /// <param name="app">The web app to register the endpoints at</param>
    public static void RegisterEndpointsFromAssembly(this WebApplication app)
    {
        var endpoints = typeof(RegisterEndpoints).Assembly.DefinedTypes
            .Where(x => x.GetInterface(nameof(IEndpoints), false) is not null && x.IsClass);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint);
            var methodInfo = endpoint.GetMethod(nameof(IEndpoints.MapEndpoints));
            methodInfo!.Invoke(instance, [app]);
        }
    }
}