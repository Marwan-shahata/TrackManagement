using TrackManagement.Application.DTOs.Artists;

namespace TrackManagement.Application.Interfaces.Services;

public interface IArtistService
{
    Task<List<ArtistResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ArtistResponse> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default);
}