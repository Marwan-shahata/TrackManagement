using TrackManagement.Domain.Entities;

public interface IArtistRepository
{
    Task<List<Artist>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Artist> AddAsync(
        Artist artist,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Artist?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default);
}