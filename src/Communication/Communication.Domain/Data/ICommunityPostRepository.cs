using Communication.Domain.Aggregates;
using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface ICommunityPostRepository
{
    Task<CommunityPost> GetWithReactionsAsync(CommunityPostId id, CancellationToken cancellationToken);

    Task<CommunityPost> GetWithCommentsAsync(CommunityPostId id, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityPostReadModel>, int)> GetByCommunityIdAsync(CommunityId communityId, UserId appUserId, int page, int pageSize, CancellationToken cancelationToken);

    Task<int> CountNewPostsAsync(CommunityId communityId, DateTimeOffset lastCheck, CancellationToken cancelationToken);

    Task<int> CountAsync(CommunityId communityId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(CommunityPostId communityPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(CommunityPostId communityPostId, CancellationToken cancellationToken);

    Task<int> CountCommentAsync(CommunityPostId communityPostId, CancellationToken cancellationToken);

    Task DeleteAsync(CommunityPostId id, CancellationToken cancelationToken);
}
