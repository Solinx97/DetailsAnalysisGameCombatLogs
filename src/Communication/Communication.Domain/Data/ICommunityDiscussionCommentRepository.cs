using Communication.Domain.Entities.Community;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionCommentRepository
{
    Task<IEnumerable<CommunityDiscussionComment>> GetByDiscussionIdAsync(int discussionId, int page, int pageSize, CancellationToken cancellationToken);
}
