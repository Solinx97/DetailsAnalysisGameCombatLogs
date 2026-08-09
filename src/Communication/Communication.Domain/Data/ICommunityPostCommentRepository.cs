using Communication.Domain.Entities.Post;

namespace Communication.Domain.Data;

public interface ICommunityPostCommentRepository
{
    Task<IEnumerable<CommunityPostComment>> GetByCommunityPostIdAsync(int communityPostId, int page, int pageSize, CancellationToken cancellationToken);
}
