using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionRepository
{
    Task<IEnumerable<CommunityDiscussion>> GetAsync(int communityId, int page, int pageSize, CancellationToken cancelationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
