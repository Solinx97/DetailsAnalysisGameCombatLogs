using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface IUserPostRepository
{
    Task<IEnumerable<UserPost>> GetByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<UserPost> GetWithLikeAsync(int id, CancellationToken cancellationToken);

    Task<UserPost> GetWithDislikeAsync(int id, CancellationToken cancellationToken);

    Task<UserPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task<int> CountAsync(string appUserId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountCommentAsync(int userPostId, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
