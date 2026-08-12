using TrackManagement.Application.DTOs.Tracks;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;
using TrackStatus = TrackManagement.Domain.Enums.TrackStatus;

namespace TrackManagement.Application.Services;

public class TrackService : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IDspRepository _dspRepository;
    public TrackService(
     ITrackRepository trackRepository,
     IArtistRepository artistRepository,
     IDspRepository dspRepository)
    {
        _trackRepository = trackRepository;
        _artistRepository = artistRepository;
        _dspRepository = dspRepository;
    }


    public async Task<TrackResponse> CreateAsync(
        CreateTrackRequest request,
        CancellationToken cancellationToken = default)
    {
        var artist =
    await _artistRepository.GetByIdAsync(
        request.ArtistId,
        cancellationToken);

        if (artist is null)
        {
            throw new ArgumentException(
                "Artist does not exist.");
        }

        var normalizedIsrc =
            request.Isrc.Trim().ToUpperInvariant();

        var isrcExists =
            await _trackRepository.IsIsrcExistsAsync(
                normalizedIsrc,
                cancellationToken);

        if (isrcExists)
        {
            throw new InvalidOperationException(
                "A track with the same ISRC already exists.");
        }

        var track = new Track
        {
            Title = request.Title.Trim(),
            ArtistId = request.ArtistId,
            Isrc = normalizedIsrc,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre.Trim(),
            Status = TrackStatus.Draft
        };

        var createdTrack =
            await _trackRepository.AddAsync(
                track,
                cancellationToken);

        return new TrackResponse
        {
            Id = createdTrack.Id,
            Title = createdTrack.Title,
            ArtistId = createdTrack.ArtistId,
            ArtistName = artist.Name,
            Isrc = createdTrack.Isrc,
            ReleaseDate = createdTrack.ReleaseDate,
            Genre = createdTrack.Genre,
            Status = createdTrack.Status.ToString()
        };
    }

    public async Task<List<TrackResponse>> GetAllAsync(
        int? artistId,
        string? genre,
        string? status,
        CancellationToken cancellationToken = default)
    {
        TrackStatus? parsedStatus = null;

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<TrackStatus>(
                    status,
                    true,
                    out var result))
            {
                throw new ArgumentException(
                    "Invalid track status.");
            }

            parsedStatus = result;
        }

        var tracks =
            await _trackRepository.GetAllAsync(
                artistId,
                genre?.Trim(),
                parsedStatus,
                cancellationToken);

        return tracks.Select(MapToResponse).ToList();
    }

    public async Task<TrackDetailsResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var track =
            await _trackRepository.GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (track is null)
            return null;

        return new TrackDetailsResponse
        {
            Id = track.Id,
            Title = track.Title,
            ArtistId = track.ArtistId,
            ArtistName = track.Artist.Name,
            Isrc = track.Isrc,
            ReleaseDate = track.ReleaseDate,
            Genre = track.Genre,
            Status = track.Status.ToString(),

            Distributions = track.Distributions
                .Select(x => new DistributionResponse
                {
                    DspId = x.DspId,
                    DspName = x.Dsp.Name,
                    SubmittedAt = x.SubmittedAt,
                    Status = x.Status.ToString()
                })
                .ToList()
        };
    }

    private static TrackResponse MapToResponse(Track track)
    {
        return new TrackResponse
        {
            Id = track.Id,
            Title = track.Title,
            ArtistId = track.ArtistId,
            ArtistName = track.Artist.Name,
            Isrc = track.Isrc,
            ReleaseDate = track.ReleaseDate,
            Genre = track.Genre,
            Status = track.Status.ToString()
        };
    }

    public async Task DistributeAsync(
    int id,
    DistributeTrackRequest request,
    CancellationToken cancellationToken = default)
    {
        var track =
            await _trackRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (track is null)
        {
            throw new ArgumentException(
                "Track does not exist.");
        }

        var requestedDspIds =
            request.DspIds
                .Distinct()
                .ToList();

        var dsps =
            await _dspRepository.GetByIdsAsync(
                requestedDspIds,
                cancellationToken);

        if (dsps.Count != requestedDspIds.Count)
        {
            throw new ArgumentException(
                "One or more DSPs do not exist.");
        }

        var distributions =
            new List<TrackDistribution>();

        foreach (var dsp in dsps)
        {
            var exists =
                await _trackRepository
                    .DistributionExistsAsync(
                        track.Id,
                        dsp.Id,
                        cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Track is already distributed to {dsp.Name}.");
            }

            distributions.Add(
                new TrackDistribution
                {
                    TrackId = track.Id,
                    DspId = dsp.Id,
                    SubmittedAt = DateTime.UtcNow,
                    Status = DistributionStatus.Pending
                });
        }

        await _trackRepository.AddDistributionsAsync(
            distributions,
            cancellationToken);

        track.Status = TrackStatus.Submitted;

        await _trackRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task UpdateStatusAsync(
    int id,
    UpdateTrackStatusRequest request,
    CancellationToken cancellationToken = default)
    {
        var track =
            await _trackRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (track is null)
        {
            throw new ArgumentException(
                "Track does not exist.");
        }

        if (!Enum.TryParse<TrackStatus>(
                request.Status,
                true,
                out var parsedStatus))
        {
            throw new ArgumentException(
                "Invalid track status.");
        }

        track.Status = parsedStatus;

        await _trackRepository.SaveChangesAsync(
            cancellationToken);
    }

}