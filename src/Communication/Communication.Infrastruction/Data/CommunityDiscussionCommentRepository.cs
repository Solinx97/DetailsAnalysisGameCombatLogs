using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityDiscussionCommentRepository(CommunicationContext context) : ICommunityDiscussionCommentRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityDiscussionComment>> GetByDiscussionIdAsync(int discussionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _context.Set<CommunityDiscussionComment>()
            .Where(x => x.CommunityDiscussionId == discussionId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return comments;
    }
}
