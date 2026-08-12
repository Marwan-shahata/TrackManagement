using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Artists.Any())
            return;

        SeedData(context);

        context.SaveChanges();
    }

    public static async Task SeedAsync(
        ApplicationDbContext context,
        CancellationToken cancellationToken = default)
    {
        if (await context.Artists.AnyAsync(cancellationToken))
            return;

        SeedData(context);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static void SeedData(ApplicationDbContext context)
    {
        var artist1 = new Artist
        {
            Name = "Lena Carter",
            Email = "lena.carter@example.com",
            Country = "United States"
        };

        var artist2 = new Artist
        {
            Name = "Omar Nile",
            Email = "omar.nile@example.com",
            Country = "Egypt"
        };

        var artist3 = new Artist
        {
            Name = "The Midnight Waves",
            Email = "midnight.waves@example.com",
            Country = "United Kingdom"
        };

        context.Artists.AddRange(
            artist1,
            artist2,
            artist3
        );

        var spotify = new Dsp
        {
            Name = "Spotify"
        };

        var appleMusic = new Dsp
        {
            Name = "Apple Music"
        };

        var youtube = new Dsp
        {
            Name = "YouTube"
        };

        context.Dsps.AddRange(
            spotify,
            appleMusic,
            youtube
        );

        var tracks = new List<Track>
        {
            new()
            {
                Title = "Midnight Drive",
                Artist = artist1,
                Isrc = "USAAA2600001",
                ReleaseDate = new DateOnly(2026, 1, 15),
                Genre = "Pop",
                Status = TrackStatus.Distributed
            },

            new()
            {
                Title = "Broken Lights",
                Artist = artist1,
                Isrc = "USAAA2600002",
                ReleaseDate = new DateOnly(2026, 2, 10),
                Genre = "Rock",
                Status = TrackStatus.Submitted
            },

            new()
            {
                Title = "Cairo Nights",
                Artist = artist2,
                Isrc = "EGAAA2600001",
                ReleaseDate = new DateOnly(2026, 3, 20),
                Genre = "Electronic",
                Status = TrackStatus.Distributed
            },

            new()
            {
                Title = "Desert Echo",
                Artist = artist2,
                Isrc = "EGAAA2600002",
                ReleaseDate = new DateOnly(2026, 4, 5),
                Genre = "Hip Hop",
                Status = TrackStatus.Draft
            },

            new()
            {
                Title = "River",
                Artist = artist2,
                Isrc = "EGAAA2600003",
                ReleaseDate = new DateOnly(2026, 5, 18),
                Genre = "R&B",
                Status = TrackStatus.Submitted
            },

            new()
            {
                Title = "Neon Skies",
                Artist = artist3,
                Isrc = "GBAAA2600001",
                ReleaseDate = new DateOnly(2026, 6, 1),
                Genre = "Alternative",
                Status = TrackStatus.Distributed
            },

            new()
            {
                Title = "After Hours",
                Artist = artist3,
                Isrc = "GBAAA2600002",
                ReleaseDate = new DateOnly(2026, 6, 25),
                Genre = "Electronic",
                Status = TrackStatus.Draft
            },

            new()
            {
                Title = "Ocean Signal",
                Artist = artist3,
                Isrc = "GBAAA2600003",
                ReleaseDate = new DateOnly(2026, 7, 12),
                Genre = "Rock",
                Status = TrackStatus.Submitted
            }
        };

        context.Tracks.AddRange(tracks);

        context.TrackDistributions.AddRange(
            new TrackDistribution
            {
                Track = tracks[0],
                Dsp = spotify,
                SubmittedAt = new DateTime(2026, 1, 10, 10, 0, 0),
                Status = DistributionStatus.Live
            },

            new TrackDistribution
            {
                Track = tracks[0],
                Dsp = appleMusic,
                SubmittedAt = new DateTime(2026, 1, 10, 10, 5, 0),
                Status = DistributionStatus.Live
            },

            new TrackDistribution
            {
                Track = tracks[1],
                Dsp = spotify,
                SubmittedAt = new DateTime(2026, 2, 8, 14, 0, 0),
                Status = DistributionStatus.Pending
            },

            new TrackDistribution
            {
                Track = tracks[2],
                Dsp = spotify,
                SubmittedAt = new DateTime(2026, 3, 15, 9, 0, 0),
                Status = DistributionStatus.Live
            },

            new TrackDistribution
            {
                Track = tracks[2],
                Dsp = youtube,
                SubmittedAt = new DateTime(2026, 3, 15, 9, 10, 0),
                Status = DistributionStatus.Live
            },

            new TrackDistribution
            {
                Track = tracks[4],
                Dsp = appleMusic,
                SubmittedAt = new DateTime(2026, 5, 15, 11, 30, 0),
                Status = DistributionStatus.Pending
            },

            new TrackDistribution
            {
                Track = tracks[5],
                Dsp = spotify,
                SubmittedAt = new DateTime(2026, 5, 28, 16, 0, 0),
                Status = DistributionStatus.Live
            },

            new TrackDistribution
            {
                Track = tracks[5],
                Dsp = youtube,
                SubmittedAt = new DateTime(2026, 5, 28, 16, 15, 0),
                Status = DistributionStatus.Rejected
            }
        );
    }
}