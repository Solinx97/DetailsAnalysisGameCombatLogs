using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserPostRepository(CommunicationContext context) : IUserPostRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<UserPost> GetWithReactionsAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<UserPost>()
            .Where(x => x.Id == id)
            .Include(x => x.UserPostLikes)
            .Include(x => x.UserPostDislikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<IEnumerable<UserPost>> GetByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var post = await _context.Set<UserPost>()
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return post;
    }

    public async Task<UserPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken)
    {
        var post = await _context.Set<UserPost>()
            .Where(x => x.Id == id)
            .Include(x => x.UserPostComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<int> CountAsync(string appUserId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<UserPost>()
            .Where(x => x.AppUserId == appUserId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountLikeAsync(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<UserPostLike>()
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountDislikeAsync(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<UserPostDislike>()
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountCommentAsync(int userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<UserPostComment>()
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<UserPost>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
