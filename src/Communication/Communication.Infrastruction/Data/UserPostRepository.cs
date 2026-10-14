using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Enums;
using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserPostRepository(CommunicationContext context) : IUserPostRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<UserPost> GetWithReactionsAsync(UserPostId id, CancellationToken cancellationToken)
    {
        var post = await _context.UserPost
            .Where(x => x.Id == id)
            .Include(x => x.UserPostLikes)
            .Include(x => x.UserPostDislikes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<(IEnumerable<UserPostReadModel>, int)> GetByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.UserPost
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId);

        var posts = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserPostReadModel(
                x.Id,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,

                x.UserPostLikes.Count(),
                x.UserPostDislikes.Count(),
                x.UserPostComments.Count(),

                x.UserPostLikes.Any(l => l.AppUserId == appUserId)
                    ? (int)PostReaction.Like
                    : x.UserPostDislikes.Any(d => d.AppUserId == appUserId)
                        ? (int)PostReaction.Dislike
                        : (int)PostReaction.None)
            )
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (posts, count);
    }

    public async Task<UserPost> GetWithCommentsAsync(UserPostId id, CancellationToken cancellationToken)
    {
        var post = await _context.UserPost
            .Where(x => x.Id == id)
            .Include(x => x.UserPostComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return post;
    }

    public async Task<int> CountAsync(UserId appUserId, CancellationToken cancellationToken)
    {
        var count = await _context.UserPost
            .Where(x => x.AppUserId == appUserId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountLikeAsync(UserPostId userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.UserPostLike
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountDislikeAsync(UserPostId userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.UserPostDislike
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task<int> CountCommentAsync(UserPostId userPostId, CancellationToken cancellationToken)
    {
        var count = await _context.UserPostComment
            .Where(x => x.UserPostId == userPostId)
            .CountAsync(cancellationToken);

        return count;
    }

    public async Task DeleteAsync(UserPostId id, CancellationToken cancelationToken)
    {
        await _context.UserPost
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
