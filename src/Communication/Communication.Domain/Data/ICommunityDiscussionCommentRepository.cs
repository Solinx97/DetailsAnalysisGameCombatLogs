using Communication.Domain.Entities.Community;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionCommentRepository
{
    Task<(IEnumerable<CommunityDiscussionComment>, int)> GetByDiscussionIdAsync(CommunityDiscussionId discussionId, int page, int pageSize, CancellationToken cancellationToken);
}
