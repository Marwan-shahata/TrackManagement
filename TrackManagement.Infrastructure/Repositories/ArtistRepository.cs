using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Data;

namespace TrackManagement.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly ApplicationDbContext _context;

    public ArtistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Artist>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Artist> AddAsync(
        Artist artist,
        CancellationToken cancellationToken = default)
    {
        _context.Artists.Add(artist);

        await _context.SaveChangesAsync(cancellationToken);

        return artist;
    }


    public async Task<bool> ExistsAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Artist?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

}