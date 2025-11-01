using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechAds.Domain.Entities;

namespace TechAds.Infrastructure;

public class TechAdsDbContext : IdentityDbContext
{
    public TechAdsDbContext(DbContextOptions<TechAdsDbContext> options) : base(options) { }

    public DbSet<ProjectListing> ProjectListings { get; set; }
    public DbSet<TechAds.Domain.Entities.Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ProjectListing>().HasKey(p => p.Id);
        builder.Entity<ProjectListing>().OwnsMany(p => p.Tags);
        builder.Entity<TechAds.Domain.Entities.Application>().HasKey(a => a.Id);
    }
}