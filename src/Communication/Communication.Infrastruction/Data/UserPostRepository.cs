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

    public async Task<UserPost> GetWithLikeAsync(int id, CancellationToken cancellationToken)
    {
        var userPost = await _context.Set<UserPost>()
            .Where(x => x.Id == id)
            .Include(x => x.UserPostLikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return userPost;
    }

    public async Task<UserPost> GetWithDislikeAsync(int id, CancellationToken cancellationToken)
    {
        var userPost = await _context.Set<UserPost>()
            .Where(x => x.Id == id)
            .Include(x => x.UserPostDislikes)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return userPost;
    }

    public async Task<UserPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken)
    {
        var userPost = await _context.Set<UserPost>()
            .Where(x => x.Id == id)
            .Include(x => x.UserPostComments)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(UserPost), id);

        return userPost;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<UserPost>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
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
}
