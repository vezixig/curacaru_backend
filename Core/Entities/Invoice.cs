namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Enums;
using Microsoft.EntityFrameworkCore;

/// <summary>Class representing an invoice created for a deployment report.</summary>
public class Invoice
{
    /// <summary>Gets or sets the id of the company owning the invoice.</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the deployment report the invoice is base ond.</summary>
    public required DeploymentReport DeploymentReport { get; set; }

    /// <summary>Gets or sets the id of the deployment report the invoice is based on.</summary>
    public Guid DeploymentReportId { get; set; }

    /// <summary>Gets or sets the company's hourly rate at the time of the invoice creation.</summary>
    public decimal HourlyRate { get; set; }

    /// <summary>Gets or sets the id of the invoice.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the date of the invoice.</summary>
    public DateOnly InvoiceDate { get; set; }

    /// <summary>Gets or sets the number of the invoice.</summary>
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = "";

    /// <summary>Gets or sets the total amount of the invoice.</summary>
    public decimal InvoiceTotal { get; set; }

    /// <summary>Gets or sets the company's ride costs at the time of the invoice creation.</summary>
    public decimal RideCosts { get; set; }

    /// <summary>Gets or sets the company's type of the ride costs at the time of the invoice creation.</summary>
    public RideCostsType RideCostsType { get; set; }

    /// <summary>Gets or sets the signature of the employee who signed the invoice.</summary>
    public string Signature { get; set; } = "";

    /// <summary>Gets or sets the employee who signed the invoice.</summary>
    public required Employee SignedEmployee { get; set; }

    /// <summary>Gets or sets the id of the employee who signed the invoice.</summary>
    public Guid SignedEmployeeId { get; set; }

    /// <summary>Gets or sets the total costs of the rides.</summary>
    public decimal TotalRideCosts { get; set; }

    /// <summary>Gets or sets the hours of work time.</summary>
    public decimal WorkedHours { get; set; }

    public static void RegisterEntity(ModelBuilder modelBuilder)
    {
        // primary key
        modelBuilder.Entity<Invoice>()
            .HasKey(o => o.Id);

        // foreign keys
        modelBuilder.Entity<Invoice>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<Invoice>()
            .HasOne(o => o.SignedEmployee)
            .WithMany()
            .HasForeignKey(o => o.SignedEmployeeId);

        modelBuilder.Entity<Invoice>()
            .HasOne(o => o.DeploymentReport)
            .WithOne(o => o.Invoice);
    }
}