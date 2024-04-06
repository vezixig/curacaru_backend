namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;
using Microsoft.EntityFrameworkCore;

/// <summary>Class representing a report of deployments at a customer.</summary>
public class DeploymentReport
{
    /// <summary>Gets or sets the associated appointments.</summary>
    public List<Appointment> Appointments { get; set; } = [];

    /// <summary>Gets or sets the care level at the time of creation.</summary>
    /// <remarks>This is a copy to save the state.</remarks>
    public int CareLevel { get; set; }

    /// <summary>Gets or sets the clearance type of the report.</summary>
    public ClearanceType ClearanceType { get; set; }

    /// <summary>Gets or sets the company id.</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public required Customer Customer { get; set; }

    /// <summary>Gets or sets the address of the customer at the time of creation.</summary>
    /// <remarks>This is a copy to save the state.</remarks>
    public string CustomerAddress { get; set; } = "";

    /// <summary>Gets or sets the customer id.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the customer's insurance status at the time of creation.</summary>
    public InsuranceStatus? CustomerInsuranceStatus { get; set; }

    /// <summary>Gets or sets the name of the customer at the time of creation.</summary>
    /// <remarks>This is a copy to save the state.</remarks>
    public string CustomerName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    /// <summary>Gets or sets the insurance at the time of creation.</summary>
    public required Insurance? Insurance { get; set; }

    /// <summary>Gets or sets the insurance id.</summary>
    public Guid? InsuranceId { get; set; }

    /// <summary>Gets or sets the insurance number at the time of creation.</summary>
    /// <remarks>This is a copy to save the state.</remarks>
    public string InsuredPersonNumber { get; set; } = "";

    /// <summary>Gets or sets the invoice for the report. Null if not created yet.</summary>
    public Invoice? Invoice { get; set; }

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
            .HasOne(o => o.Insurance)
            .WithMany()
            .HasForeignKey(o => o.InsuranceId);

        modelBuilder.Entity<DeploymentReport>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<DeploymentReport>()
            .HasMany(o => o.Appointments)
            .WithOne()
            .HasForeignKey(o => o.DeploymentReportId);

        // unique constraint
        modelBuilder.Entity<DeploymentReport>()
            .HasIndex(o => new { o.CustomerId, o.Year, o.Month, o.ClearanceType })
            .IsUnique();
    }
}