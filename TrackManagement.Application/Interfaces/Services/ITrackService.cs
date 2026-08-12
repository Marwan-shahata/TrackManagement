using TrackManagement.Application.DTOs.Tracks;

namespace TrackManagement.Application.Interfaces.Services;

public interface ITrackService
{
    Task<TrackResponse> CreateAsync(
        CreateTrackRequest request,
        CancellationToken cancellationToken = default);

    Task<List<TrackResponse>> GetAllAsync(
        int? artistId,
        string? genre,
        string? status,
        CancellationToken cancellationToken = default);

    Task<TrackDetailsResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);


    Task DistributeAsync(
    int id,
    DistributeTrackRequest request,
    CancellationToken cancellationToken = default);

    Task UpdateStatusAsync(
        int id,
        UpdateTrackStatusRequest request,
        CancellationToken cancellationToken = default);

}