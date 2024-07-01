namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class ContactForm
{
    [MaxLength(11)]
    public string Color { get; set; } = null!;

    public Guid CompanyId { get; set; }

    public int FontSize { get; set; }

    public Guid Id { get; set; }

    public bool IsRounded { get; set; }

    public static void RegisterEntity(ModelBuilder modelBuilder)
    {
        // primary key
        modelBuilder.Entity<ContactForm>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<ContactForm>()
            .HasOne<Company>()
            .WithOne()
            .HasForeignKey<ContactForm>(o => o.CompanyId);
    }
}