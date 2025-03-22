using ManagementPias.App.Interfaces;
using ManagementPias.Domain.Common;
using ManagementPias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace ManagementPias.Infra.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTime;
    private readonly ILoggerFactory _loggerFactory;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
            , IDateTimeService dateTime
            , ILoggerFactory loggerFactory
            ) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTime = dateTime;
        _loggerFactory = loggerFactory;
    }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<User> User => Set<User>();

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
        //var databaseSeeder = new DatabaseSeeder();

        // Configure the tables 
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new AssetConfiguration());
        //modelBuilder.ApplyConfiguration(new UserConfiguration());

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
        builder.Property(x => x.NumUnit).IsRequired();
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
    }
}

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.Role);
        builder.Property(x => x.IsActive);
        builder.Property(x => x.IsVerified);
    }
}