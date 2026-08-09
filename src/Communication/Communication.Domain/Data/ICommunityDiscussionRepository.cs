using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionRepository
{
    Task<IEnumerable<CommunityDiscussion>> GetAsync(int communityId, int page, int pageSize, CancellationToken cancelationToken);

    Task<CommunityDiscussion> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
