using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swag.Resource.Data.SwagDb.Models;

namespace Swag.Resource.Data.SwagDb;

public class SwagContext : DbContext
{
        
    public DbSet<Estimate> Estimates { get; set; }

    public SwagContext(DbContextOptions<SwagContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        OnModelCreating(modelBuilder.Entity<Estimate>());
    }

    private void OnModelCreating(EntityTypeBuilder<Estimate> entity)
    {
        entity.ToTable(nameof(Estimates));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        entity.Property(e => e.Optimistic).IsRequired();
        entity.Property(e => e.MostLikely).IsRequired();
        entity.Property(e => e.Pessimistic).IsRequired();
        entity.Property(e => e.Calculated).HasPrecision(4);
    }

}