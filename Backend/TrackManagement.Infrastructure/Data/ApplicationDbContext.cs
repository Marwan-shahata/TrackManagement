using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists => Set<Artist>();

    public DbSet<Track> Tracks => Set<Track>();

    public DbSet<Dsp> Dsps => Set<Dsp>();

    public DbSet<TrackDistribution> TrackDistributions
        => Set<TrackDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}