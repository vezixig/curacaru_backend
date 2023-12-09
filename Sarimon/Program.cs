using Curacaru.Backend.Application;
using Curacaru.Backend.Infrastructure;
using Curacaru.Backend.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CompanyMiddleware>();

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
app.UseAuthorization();
app.UseMiddleware<CompanyMiddleware>();
app.MapControllers();

app.Run();