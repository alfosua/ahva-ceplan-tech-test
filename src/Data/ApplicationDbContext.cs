using Ahva.Ceplan.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ahva.Ceplan.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserProfile>(profile =>
        {
            profile.HasIndex(p => p.UserId).IsUnique();

            profile.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            profile.HasIndex(p => new { p.DocumentType, p.DocumentNumber }).IsUnique();

            profile.Property(p => p.FirstNames).HasMaxLength(128);
            profile.Property(p => p.LastNamePaternal).HasMaxLength(128);
            profile.Property(p => p.LastNameMaternal).HasMaxLength(128);
            profile.Property(p => p.DocumentType).HasMaxLength(8);
            profile.Property(p => p.DocumentNumber).HasMaxLength(16);
            profile.Property(p => p.Nationality).HasMaxLength(64);
            profile.Property(p => p.Sex).HasMaxLength(16);
            profile.Property(p => p.SecondaryEmail).HasMaxLength(256);
            profile.Property(p => p.MobilePhone).HasMaxLength(32);
            profile.Property(p => p.SecondaryPhoneType).HasMaxLength(32);
            profile.Property(p => p.SecondaryPhone).HasMaxLength(32);
            profile.Property(p => p.ContractType).HasMaxLength(64);
            profile.Property(p => p.JobTitle).HasMaxLength(128);
            profile.Property(p => p.Entity).HasMaxLength(128);
            profile.Property(p => p.PideValidationStatus).HasMaxLength(32);
        });
    }
}
