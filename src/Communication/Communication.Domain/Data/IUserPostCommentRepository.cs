using Communication.Domain.Entities.Post;

namespace Communication.Domain.Data;

public interface IUserPostCommentRepository
{
    Task<IEnumerable<UserPostComment>> GetByUserPostIdAsync(int userPostId, int page, int pageSize, CancellationToken cancellationToken);
}
