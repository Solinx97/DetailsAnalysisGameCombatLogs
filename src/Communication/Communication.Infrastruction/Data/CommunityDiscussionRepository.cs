using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityDiscussionRepository(CommunicationContext context) : ICommunityDiscussionRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<(IEnumerable<CommunityDiscussion>, int)> GetAsync(CommunityId communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CommunityDiscussion
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

    public async Task<CommunityDiscussion> GetWithCommentsAsync(CommunityDiscussionId id, CancellationToken cancellationToken)
    {
        var communityDiscussions = await _context.CommunityDiscussion
            .Where(x => x.Id == id)
            .Include(x => x.CommunityDiscussionComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(CommunityDiscussion), id);

        return communityDiscussions;
    }

    public async Task DeleteAsync(CommunityDiscussionId id, CancellationToken cancelationToken)
    {
        await _context.CommunityDiscussion
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
