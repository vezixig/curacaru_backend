using Curacaru.Backend.Application;
using Curacaru.Backend.Infrastructure;
using Curacaru.Backend.Middleware;
using Curacaru.Backend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();
builder.Services.AddSingleton<CompanyMiddleware>();
builder.Services.AddSingleton<SlowLoaderMiddleware>();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateLogger();

try
{
    // Add Cors
    builder.Services.AddSerilog();
    builder.Services.AddHttpLogging(o => { });
    builder.Services.AddCors(
        options =>
        {
            options.AddPolicy(
                "local",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

    // Add Auth
    builder.Services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(
            options =>
            {
                options.Authority = Environment.GetEnvironmentVariable("IDENTITY_AUTHORITY");
                options.Audience = Environment.GetEnvironmentVariable("IDENTITY_AUDIENCE");
            });

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("Company", policy => { policy.RequireClaim("CompanyId"); })
        .AddPolicy("Manager", policy => { policy.RequireClaim("Manager"); });

    // Add app layers
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();

    var app = builder.Build();
    app.UseHttpLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("local");
        //app.UseMiddleware<SlowLoaderMiddleware>();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.UseMiddleware<CompanyMiddleware>();
    app.UseAuthorization();
    app.MapControllers();
    app.RegisterEndpointsFromAssembly();

    app.Run();
}
catch (Exception ex) { Log.Fatal(ex, "Application terminated unexpectedly"); }
finally { Log.CloseAndFlush(); }