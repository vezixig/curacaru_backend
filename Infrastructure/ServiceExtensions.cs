namespace Curacaru.Backend.Infrastructure;

using Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Service collection extension to register repositories from the assembly.</summary>
internal static class RegisterRepositories
{
    /// <summary>Scans the assembly for classes with the <see cref="RepositoryAttribute" /> and registers them at the app.</summary>
    /// <param name="services">The service collection to register the repositories at</param>
    public static void AddRepositoriesFromAssembly(this IServiceCollection services)
    {
        var repositories = typeof(RegisterRepositories).Assembly.DefinedTypes
            .Where(o => o.GetCustomAttributes(typeof(RepositoryAttribute), false).Length > 0 && o.IsClass);

        var addScopedMethod = typeof(ServiceCollectionServiceExtensions).GetMethod("AddScoped", [typeof(IServiceCollection), typeof(Type), typeof(Type)]);
        foreach (var repository in repositories)
        {
            var serviceType = repository.GetInterfaces()[0];
            var implementationType = repository.AsType();

            addScopedMethod!.Invoke(null, [services, serviceType, implementationType]);
        }
    }
}