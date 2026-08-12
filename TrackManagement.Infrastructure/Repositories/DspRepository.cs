using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Data;

namespace TrackManagement.Infrastructure.Repositories;

public class DspRepository : IDspRepository
{
    private readonly ApplicationDbContext _context;

    public DspRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Dsp>> GetByIdsAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        var dspIds = ids.Distinct().ToList();

        return await _context.Dsps
            .Where(x => dspIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }
}