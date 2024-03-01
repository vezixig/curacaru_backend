namespace Curacaru.Backend.Application;

using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Implementations;

/// <summary>Dependency injection registration for the application layer.</summary>
public static class ServiceRegistration
{
    /// <summary>Adds the application layer to the dependency injection container.</summary>
    /// <param name="services">The services.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<IBudgetService, BudgetService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        services.AddAutoMapper(typeof(MappingProfile));
    }
}