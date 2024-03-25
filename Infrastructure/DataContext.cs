namespace Curacaru.Backend.Infrastructure;

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>Data context used to link users with their tenant schema.</summary>
internal class DataContext : DbContext
{
    /// <summary>Gets or sets the set of appointments.</summary>
    public DbSet<Appointment> Appointments { get; set; } = null!;

    public DbSet<AssignmentDeclaration> AssignmentDeclarations { get; set; } = null!;

    /// <summary>Gets or sets the set of budgets.</summary>
    public DbSet<Budget> Budgets { get; set; } = null!;

    /// <summary>Gets or sets the set of companies.</summary>
    public DbSet<Company> Companies { get; set; } = null!;

    /// <summary>Gets or sets the set of customers.</summary>
    public DbSet<Customer> Customers { get; set; } = null!;

    /// <summary>Gets or sets the set of deployment reports.</summary>
    public DbSet<DeploymentReport> DeploymentReports { get; set; } = null!;

    /// <summary>Gets or sets the set of employees.</summary>
    public DbSet<Employee> Employees { get; set; } = null!;

    /// <summary>Gets or sets the set of insurances.</summary>
    public DbSet<Insurance> Insurances { get; set; } = null!;

    /// <summary>Gets or sets the set of working time reports.</summary>
    public DbSet<WorkingTimeReport> WorkingTimeReports { get; set; } = null!;

    /// <summary>Gets or sets the set of zip codes.</summary>
    public DbSet<ZipCity> ZipCities { get; set; } = null!;

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

        modelBuilder.Entity<Appointment>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Appointment>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Appointment>()
            .HasOne(o => o.Employee)
            .WithMany()
            .HasForeignKey(o => o.EmployeeId);

        modelBuilder.Entity<Appointment>()
            .HasOne(o => o.EmployeeReplacement)
            .WithMany()
            .HasForeignKey(o => o.EmployeeReplacementId);

        // todo:  assembly  scanning?
        AssignmentDeclaration.RegisterEntity(modelBuilder);
        DeploymentReport.RegisterEntity(modelBuilder);

        modelBuilder.Entity<Budget>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Budget>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Employee>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Company>()
            .HasOne(o => o.ZipCity)
            .WithMany()
            .HasForeignKey(o => o.ZipCode);

        modelBuilder.Entity<Customer>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Customer>()
            .HasOne(o => o.Insurance)
            .WithMany()
            .HasForeignKey(o => o.InsuranceId);

        modelBuilder.Entity<Customer>()
            .HasOne(o => o.AssociatedEmployee)
            .WithMany()
            .HasForeignKey(o => o.AssociatedEmployeeId);

        modelBuilder.Entity<Customer>()
            .HasOne(o => o.ZipCity)
            .WithMany()
            .HasForeignKey(o => o.ZipCode);

        modelBuilder.Entity<Insurance>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Insurance>()
            .HasOne(o => o.ZipCity)
            .WithMany()
            .HasForeignKey(o => o.ZipCode);

        modelBuilder.Entity<WorkingTimeReport>()
            .HasOne(o => o.Employee)
            .WithMany()
            .HasForeignKey(o => o.EmployeeId);

        modelBuilder.Entity<WorkingTimeReport>()
            .HasOne(o => o.Manager)
            .WithMany()
            .HasForeignKey(o => o.ManagerId);

        modelBuilder.Entity<WorkingTimeReport>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<WorkingTimeReport>()
            .HasIndex(o => new { o.EmployeeId, o.Year, o.Month })
            .IsUnique();
    }
}