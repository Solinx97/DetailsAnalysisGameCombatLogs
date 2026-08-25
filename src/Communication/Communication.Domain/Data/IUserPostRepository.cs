using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Post;
using Communication.Domain.ReadModel;

namespace Communication.Domain.Data;

public interface IUserPostRepository
{
    Task<(IEnumerable<UserPostReadModel>, int)> GetByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<UserPost> GetWithReactionsAsync(int id, CancellationToken cancellationToken);

    Task<UserPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task<UserPostLike> GetLikeByIdAsync(int id, CancellationToken cancellationToken);

    Task<int> CountAsync(string appUserId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountCommentAsync(int userPostId, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
