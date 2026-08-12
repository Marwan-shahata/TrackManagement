using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Data;
using TrackStatus = TrackManagement.Domain.Enums.TrackStatus;

namespace TrackManagement.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly ApplicationDbContext _context;

    public TrackRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Track> AddAsync(
        Track track,
        CancellationToken cancellationToken = default)
    {
        _context.Tracks.Add(track);

        await _context.SaveChangesAsync(cancellationToken);

        return track;
    }

    public async Task<bool> IsIsrcExistsAsync(
        string isrc,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tracks
            .AnyAsync(
                x => x.Isrc == isrc,
                cancellationToken);
    }

    public async Task<List<Track>> GetAllAsync(
        int? artistId,
        string? genre,
        TrackStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tracks
            .AsNoTracking()
            .Include(x => x.Artist)
            .AsQueryable();

        if (artistId.HasValue)
        {
            query = query.Where(
                x => x.ArtistId == artistId.Value);
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(
                x => x.Genre == genre);
        }

        if (status.HasValue)
        {
            query = query.Where(
                x => x.Status == status.Value);
        }

        return await query
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Track?> GetByIdWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tracks
            .AsNoTracking()
            .Include(x => x.Artist)
            .Include(x => x.Distributions)
                .ThenInclude(x => x.Dsp)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    public async Task<Track?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        return await _context.Tracks
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> DistributionExistsAsync(
        int trackId,
        int dspId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TrackDistributions
            .AnyAsync(
                x => x.TrackId == trackId &&
                     x.DspId == dspId,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddDistributionsAsync(
    IEnumerable<TrackDistribution> distributions,
    CancellationToken cancellationToken = default)
    {
        await _context.TrackDistributions
            .AddRangeAsync(
                distributions,
                cancellationToken);
    }
}