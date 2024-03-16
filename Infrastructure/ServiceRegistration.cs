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
    /// <param name="services">The service collection.</param>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Add database context
        services.AddDbContext<DataContext>();

        // Add repositories
        // todo: use assembly scanning
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IWorkingTimeRepository, WorkingTimeReportRepository>();

        // Add services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<IReportService, ReportService>();
    }
}