using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityPostRepository(CommunicationContext context) : ICommunityPostRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<CommunityPost> GetWithReactionsAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<CommunityPost>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityPostLikes)
            .Include(x => x.CommunityPostDislikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(CommunityPost), id);

        return post;
    }

    public async Task<CommunityPost> GetWithLikeAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<CommunityPost>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityPostLikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<CommunityPost> GetWithDislikeAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<CommunityPost>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityPostDislikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<CommunityPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<CommunityPost>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityPostComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<IEnumerable<CommunityPost>> GetByCommunityIdAsync(int communityId, int page, int pageSize, CancellationToken cancelationToken)
    {
        var result = await _context.Set<CommunityPost>()
            .AsNoTracking()
            .Where(x => x.CommunityId == communityId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancelationToken);
        return result.Count != 0 ? result : [];
    }

    public async Task<int> CountAsync(int communityId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<CommunityPost>()
            .Where(x => x.CommunityId == communityId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountLikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<CommunityPostLike>()
            .Where(x => x.CommunityPostId == communityPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountDislikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<CommunityPostDislike>()
            .Where(x => x.CommunityPostId == communityPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountCommentAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<CommunityPostComment>()
            .Where(x => x.CommunityPostId == communityPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<CommunityPost>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
