using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure.Data.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Tracks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Isrc)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.Isrc)
            .IsUnique();

        builder.Property(x => x.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasOne(x => x.Artist)
            .WithMany(x => x.Tracks)
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}