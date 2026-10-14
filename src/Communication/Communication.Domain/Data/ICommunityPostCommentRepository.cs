using Communication.Domain.Entities.Post;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface ICommunityPostCommentRepository
{
    Task<(IEnumerable<CommunityPostComment>, int)> GetByCommunityPostIdAsync(CommunityPostId communityPostId, int page, int pageSize, CancellationToken cancellationToken);
}
