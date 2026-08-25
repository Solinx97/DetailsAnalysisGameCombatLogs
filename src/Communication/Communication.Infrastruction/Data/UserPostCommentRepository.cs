using Communication.Domain.Data;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserPostCommentRepository(CommunicationContext context) : IUserPostCommentRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<(IEnumerable<UserPostComment>, int)> GetByUserPostIdAsync(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<UserPostComment>()
            .AsNoTracking()
            .Where(x => x.UserPostId == userPostId);

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
