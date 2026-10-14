using Communication.Domain.Aggregates;
using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface IUserPostRepository
{
    Task<(IEnumerable<UserPostReadModel>, int)> GetByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<UserPost> GetWithReactionsAsync(UserPostId id, CancellationToken cancellationToken);

    Task<UserPost> GetWithCommentsAsync(UserPostId id, CancellationToken cancellationToken);

    Task<int> CountAsync(UserId appUserId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(UserPostId userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(UserPostId userPostId, CancellationToken cancellationToken);

    Task<int> CountCommentAsync(UserPostId userPostId, CancellationToken cancellationToken);

    Task DeleteAsync(UserPostId id, CancellationToken cancelationToken);
}
