using Communication.Domain.Entities.Post;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface IUserPostCommentRepository
{
    Task<(IEnumerable<UserPostComment>, int)> GetByUserPostIdAsync(UserPostId userPostId, int page, int pageSize, CancellationToken cancellationToken);
}
