using Communication.Domain.Data;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityPostCommentRepository(CommunicationContext context) : ICommunityPostCommentRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityPostComment>> GetByCommunityPostIdAsync(int communityPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _context.Set<CommunityPostComment>()
            .Where(x => x.CommunityPostId == communityPostId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return comments;
    }
}
