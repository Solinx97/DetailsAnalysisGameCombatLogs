using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface ICommunityDiscussionRepository
{
    Task<IEnumerable<CommunityDiscussion>> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityDiscussion>, int)> GetAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityDiscussion> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
