namespace Curacaru.Backend.Application;

using Microsoft.Extensions.DependencyInjection;

/// <summary>Dependency injection registration for the application layer.</summary>
public static class ServiceRegistration
{
    /// <summary>Adds the application layer to the dependency injection container.</summary>
    /// <param name="services">The services.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
    }
}