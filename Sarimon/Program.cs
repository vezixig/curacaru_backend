using System.Security.Authentication;
using System.Security.Claims;
using Curacaru.Backend.Application;
using Curacaru.Backend.Application.CQRS.TimeTracking;
using Curacaru.Backend.Core.Enums;
using Curacaru.Backend.Infrastructure;
using Curacaru.Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();
builder.Services.AddSingleton<CompanyMiddleware>();

// Add Cors
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("local");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<CompanyMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.MapGet(
        "employee/{employeeId:guid}/work-time",
        async (
            IMediator mediator,
            ClaimsPrincipal principal,
            Guid employeeId,
            int month,
            int year) => await mediator.Send(new WorkingHoursRequest(employeeId, GetCompanyId(principal), GetAuthId(principal), month, year)))
    .RequireAuthorization(Policy.Company);

app.Run();

static Guid GetCompanyId(ClaimsPrincipal claimsPrincipal)
    => claimsPrincipal.Claims.Any(o => o.Type == "CompanyId")
        ? Guid.Parse(claimsPrincipal.Claims.First(o => o.Type == "CompanyId").Value)
        : throw new AuthenticationException("User is not associated with a company.");

static string GetAuthId(ClaimsPrincipal claimsPrincipal)
    => claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;