using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionRepository
{
    Task<(IEnumerable<CommunityDiscussion>, int)> GetAsync(CommunityId communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityDiscussion> GetWithCommentsAsync(CommunityDiscussionId id, CancellationToken cancellationToken);

    Task DeleteAsync(CommunityDiscussionId id, CancellationToken cancelationToken);
}
