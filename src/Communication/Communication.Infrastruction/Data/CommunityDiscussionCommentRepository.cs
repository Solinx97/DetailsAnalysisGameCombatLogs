using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityDiscussionCommentRepository(CommunicationContext context) : ICommunityDiscussionCommentRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<(IEnumerable<CommunityDiscussionComment>, int)> GetByDiscussionIdAsync(int discussionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<CommunityDiscussionComment>()
            .AsNoTracking()
            .Where(x => x.CommunityDiscussionId == discussionId);

        var comments = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (comments, count);
    }
}
