using ManagamentPias.App.Interfaces;
using ManagamentPias.Domain.Common;
using ManagamentPias.Domain.Entities;
using ManagamentPias.Infra.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace ManagamentPias.Infra.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTime;
    private readonly ILoggerFactory _loggerFactory;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
            //,IDateTimeService dateTime,
            //ILoggerFactory loggerFactory
            ) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //_dateTime = dateTime;
        //_loggerFactory = loggerFactory;
    }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<Rating> Ratings => Set<Rating>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.NowUtc;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = _dateTime.NowUtc;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Generate seed data with Bogus
        var databaseSeeder = new DatabaseSeeder();
        // Apply the seed data on the tables
        modelBuilder.Entity<Portfolio>().HasData(databaseSeeder.Portfolio);

        // Configure the tables for the Warehouse
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new AssetConfiguration());
        modelBuilder.ApplyConfiguration(new PortfolioConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }
}


internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ValuePatrimony).IsRequired().HasPrecision(12, 2);
        builder.Property(x => x.NumUnit).IsRequired();

        builder.HasOne(b => b.Rating)
            .WithMany()
            .HasForeignKey(x => x.RatingId);
    }
}

internal class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Valuation).IsRequired().HasPrecision(12, 2);
        builder.Property(x => x.DateSituation).IsRequired();
        builder.HasOne(x => x.Portfolio)
            .WithMany()
            .HasForeignKey(x => x.PortfolioId);
    }
}

internal class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("Portfolios");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).IsRequired();
    }
}