using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using UniMagazine.Models;

namespace UniMagazine.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<Magazine> Magazines { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    public DbSet<MaterialContribution> MaterialContributions { get; set; }
    public DbSet<FeedbackComment> FeedbackComments { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Magazine>()
                .HasMany(e => e.Contributions)
                .WithOne(e => e.Magazine)
                .HasForeignKey("MagazineId")
                .IsRequired();
        builder.Entity<Contribution>()
        .HasOne(c => c.User)
        .WithMany(u => u.Contributions)
        .HasForeignKey(c => c.UserId);
    }
}
