using TrackManagement.Application.DTOs.Artists;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<List<ArtistResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var artists =
            await _artistRepository.GetAllAsync(cancellationToken);

        return artists.Select(MapToResponse).ToList();
    }

    public async Task<ArtistResponse> CreateAsync(
        CreateArtistRequest request,
        CancellationToken cancellationToken = default)
    {
        var artist = new Artist
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Country = request.Country.Trim()
        };

        var createdArtist =
            await _artistRepository.AddAsync(
                artist,
                cancellationToken);

        return MapToResponse(createdArtist);
    }

    private static ArtistResponse MapToResponse(Artist artist)
    {
        return new ArtistResponse
        {
            Id = artist.Id,
            Name = artist.Name,
            Email = artist.Email,
            Country = artist.Country
        };
    }
}