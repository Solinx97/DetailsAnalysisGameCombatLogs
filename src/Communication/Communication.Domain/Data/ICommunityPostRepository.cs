using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface ICommunityPostRepository
{
    Task<CommunityPost> GetWithLikeAsync(int id, CancellationToken cancellationToken);

    Task<CommunityPost> GetWithDislikeAsync(int id, CancellationToken cancellationToken);

    Task<CommunityPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<CommunityPost>> GetByCommunityIdAsync(int communityId, int page, int pageSize, CancellationToken cancelationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);

    Task<int> CountLikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int userPostId, CancellationToken cancellationToken);
}
