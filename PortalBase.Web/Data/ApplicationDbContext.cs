using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalBase.Core.Entities;

namespace PortalBase.Web.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Page> Pages { get; set; }
    public DbSet<Scholarship> Scholarships { get; set; }
    public DbSet<ContactInquiry> ContactInquiries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Page entity
        builder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.Slug).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.HeroImageUrl).HasMaxLength(1000);
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.ContentHtml).HasColumnType("longtext");
        });

        // Configure Scholarship entity
        builder.Entity<Scholarship>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.FieldOfStudy).HasMaxLength(100);
            entity.Property(e => e.DegreeLevel).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.ApplicationUrl).HasMaxLength(500);
            entity.Property(e => e.OfficialWebsite).HasMaxLength(500);
            entity.Property(e => e.AdditionalInfo).HasMaxLength(1000);
            entity.Property(e => e.Description).HasColumnType("longtext");
            entity.Property(e => e.Eligibility).HasColumnType("longtext");
            entity.Property(e => e.RequiredDocuments).HasColumnType("longtext");
        });

        // Configure ContactInquiry entity
        builder.Entity<ContactInquiry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Subject).HasMaxLength(200);
            entity.Property(e => e.Message).HasColumnType("longtext");
            entity.Property(e => e.InquiryType).HasMaxLength(50);
            entity.Property(e => e.AdminNotes).HasMaxLength(500);
        });
    }
}


