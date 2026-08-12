using TrackManagement.Domain.Entities;
using TrackStatus = TrackManagement.Domain.Enums.TrackStatus;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface ITrackRepository
{
    Task<Track> AddAsync(
        Track track,
        CancellationToken cancellationToken = default);

    Task<bool> IsIsrcExistsAsync(
        string isrc,
        CancellationToken cancellationToken = default);

    Task<List<Track>> GetAllAsync(
        int? artistId,
        string? genre,
        TrackStatus? status,
        CancellationToken cancellationToken = default);

    Task<Track?> GetByIdWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Track?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default);

    Task<bool> DistributionExistsAsync(
        int trackId,
        int dspId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task AddDistributionsAsync(
    IEnumerable<TrackDistribution> distributions,
    CancellationToken cancellationToken = default);

}