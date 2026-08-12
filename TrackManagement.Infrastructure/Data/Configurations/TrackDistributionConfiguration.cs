using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure.Data.Configurations;

public class TrackDistributionConfiguration
    : IEntityTypeConfiguration<TrackDistribution>
{
    public void Configure(
        EntityTypeBuilder<TrackDistribution> builder)
    {
        builder.ToTable("TrackDistributions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.SubmittedAt)
            .IsRequired();

        builder.HasOne(x => x.Track)
            .WithMany(x => x.Distributions)
            .HasForeignKey(x => x.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Dsp)
            .WithMany(x => x.TrackDistributions)
            .HasForeignKey(x => x.DspId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.TrackId,
            x.DspId
        }).IsUnique();
    }
}