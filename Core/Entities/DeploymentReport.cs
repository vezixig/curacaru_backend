namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;
using Microsoft.EntityFrameworkCore;

/// <summary>Class representing a report of deployments at a customer.</summary>
public class DeploymentReport
{
    /// <summary>Gets or sets the associated appointments.</summary>
    public List<Appointment> Appointments { get; set; } = [];

    /// <summary>Gets or sets the clearance type of the report.</summary>
    public ClearanceType ClearanceType { get; set; }

    /// <summary>Gets or sets the company id.</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public required Customer Customer { get; set; }

    /// <summary>Gets or sets the customer id.</summary>
    public Guid CustomerId { get; set; }

    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets the month of the report.</summary>
    public int Month { get; set; }

    /// <summary>Gets or sets the city where the signature took place.</summary>
    [MaxLength(30)]
    public string SignatureCity { get; set; } = "";

    /// <summary>Gets or sets the signature of the customer.</summary>
    public string SignatureCustomer { get; set; } = "";

    /// <summary>Gets or sets the date of the signature.</summary>
    public DateOnly SignatureDate { get; set; }

    /// <summary>Gets or sets the signature of the employee.</summary>
    public string SignatureEmployee { get; set; } = "";

    /// <summary>Gets or sets the amount of worked hours.</summary>
    public double WorkedHours { get; set; }

    /// <summary>Gets or sets the year of the report.</summary>
    public int Year { get; set; }

    public static void RegisterEntity(ModelBuilder modelBuilder)
    {
        // primary key
        modelBuilder.Entity<DeploymentReport>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<DeploymentReport>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<DeploymentReport>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<DeploymentReport>()
            .HasMany(o => o.Appointments)
            .WithOne();

        // unique constraint
        modelBuilder.Entity<DeploymentReport>()
            .HasIndex(o => new { o.CustomerId, o.Year, o.Month, o.ClearanceType })
            .IsUnique();
    }
}