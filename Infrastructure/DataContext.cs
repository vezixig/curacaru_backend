namespace Curacaru.Backend.Infrastructure;

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>Data context used to link users with their tenant schema.</summary>
internal class DataContext : DbContext
{
    /// <summary>Gets or sets the set of companies.</summary>
    public DbSet<Company> Companies { get; set; } = null!;

    /// <summary>Gets or sets the set of employees.</summary>
    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableDetailedErrors()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);
    }
}