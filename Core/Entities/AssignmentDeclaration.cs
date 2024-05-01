namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class AssignmentDeclaration
{
    /// <summary>Gets or sets the company id.</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Gets or sets the customer.</summary>
    public required Customer Customer { get; set; }

    /// <summary>Gets or sets the customer first name.</summary>
    [MaxLength(150)]
    public string CustomerFirstName { get; set; } = "";

    /// <summary>Gets or sets the customer id.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the customer last name.</summary>
    [MaxLength(150)]
    public string CustomerLastName { get; set; } = "";

    /// <summary>Gets or sets the customer street.</summary>
    [MaxLength(150)]
    public string CustomerStreet { get; set; } = "";

    /// <summary>Gets or sets the zip code and city of the customer.</summary>
    public required ZipCity CustomerZipCity { get; set; }

    /// <summary>Gets or sets the customer zip code.</summary>
    [MaxLength(5)]
    public string CustomerZipCode { get; set; } = "";

    /// <summary>Gets or sets the date of birth of the customer.</summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>Gets or sets the id of the declaration of assignment.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the id of the insurance the customer is insured at.</summary>
    public Guid InsuranceId { get; set; }

    /// <summary>Gets or sets the insurance name.</summary>
    [MaxLength(150)]
    public string InsuranceName { get; set; } = "";

    /// <summary>Gets or sets the insurance street.</summary>
    [MaxLength(150)]
    public string InsuranceStreet { get; set; } = "";

    /// <summary>Gets or sets the zip code and city of the insurance.</summary>
    public required ZipCity InsuranceZipCity { get; set; }

    /// <summary>Gets or sets the insurance zip code.</summary>
    [MaxLength(5)]
    public string InsuranceZipCode { get; set; } = "";

    /// <summary>Gets or sets the insured person number of the customer.</summary>
    [MaxLength(30)]
    public string InsuredPersonNumber { get; set; } = "";

    /// <summary>Gets or sets the signature of the customer.</summary>
    [MaxLength(100000)]
    public string Signature { get; set; } = "";

    /// <summary>Gets or sets the city of the signature.</summary>
    [MaxLength(30)]
    public string SignatureCity { get; set; } = "";

    /// <summary>Gets or sets the date of the signature.</summary>
    public DateOnly SignatureDate { get; set; }

    /// <summary>Gets or sets the year of the declaration of assignment.</summary>
    public int Year { get; set; }

    public static void RegisterEntity(ModelBuilder modelBuilder)
    {
        // primary key
        modelBuilder.Entity<AssignmentDeclaration>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<AssignmentDeclaration>()
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(o => o.CompanyId);

        modelBuilder.Entity<AssignmentDeclaration>()
            .HasOne(o => o.Customer)
            .WithMany(o => o.AssignmentDeclarations)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<AssignmentDeclaration>()
            .HasOne<Insurance>()
            .WithMany()
            .HasForeignKey(o => o.InsuranceId);

        modelBuilder.Entity<AssignmentDeclaration>()
            .HasOne(o => o.CustomerZipCity)
            .WithMany()
            .HasForeignKey(o => o.CustomerZipCode);

        modelBuilder.Entity<AssignmentDeclaration>()
            .HasOne(o => o.InsuranceZipCity)
            .WithMany()
            .HasForeignKey(o => o.InsuranceZipCode);

        // unique constraint
        modelBuilder.Entity<AssignmentDeclaration>()
            .HasIndex(o => new { o.CustomerId, o.Year })
            .IsUnique();
    }
}