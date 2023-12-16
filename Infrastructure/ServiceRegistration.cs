namespace Curacaru.Backend.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using repositories;
using Repositories;
using repositories.implementation;
using Repositories.Implementation;
using Services;
using Services.Implementations;

/// <summary>Dependency injection registration for the infrastructure layer.</summary>
public static class ServiceRegistration
{
    /// <summary>Adds the infrastructure layer to the dependency injection container.</summary>
    /// <param name="services">The services.</param>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>();

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}