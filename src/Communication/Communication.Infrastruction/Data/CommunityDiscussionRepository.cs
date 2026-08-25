using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityDiscussionRepository(CommunicationContext context) : ICommunityDiscussionRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityDiscussion>> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken)
    {
        var discussions = await _context.Set<CommunityDiscussion>()
            .AsNoTracking()
            .Where(x => x.CommunityId == communityId)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return discussions;
    }

    public async Task<(IEnumerable<CommunityDiscussion>, int)> GetAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<CommunityDiscussion>()
            .AsNoTracking()
            .Where(x => x.CommunityId == communityId);

        var discussions = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (discussions, count);
    }

    public async Task<CommunityDiscussion> GetWithCommentsAsync(int id, CancellationToken cancellationToken)
    {
        var communityDiscussions = await _context.Set<CommunityDiscussion>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityDiscussionComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(CommunityDiscussion), id);

        return communityDiscussions;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<CommunityDiscussion>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
