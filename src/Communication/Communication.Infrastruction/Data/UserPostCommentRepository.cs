using Communication.Domain.Data;
using Communication.Domain.Entities.Post;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserPostCommentRepository(CommunicationContext context) : IUserPostCommentRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<UserPostComment>> GetByUserPostIdAsync(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var comments = await _context.Set<UserPostComment>()
            .Where(x => x.UserPostId == userPostId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return comments;
    }
}
