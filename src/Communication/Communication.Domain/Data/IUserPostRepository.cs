using Communication.Domain.Aggregates;

namespace Communication.Domain.Data;

public interface IUserPostRepository
{
    Task<UserPost> GetWithLikeAsync(int id, CancellationToken cancellationToken);

    Task<UserPost> GetWithDislikeAsync(int id, CancellationToken cancellationToken);

    Task<UserPost> GetWithCommentsAsync(int id, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);

    Task<int> CountLikeAsync(int userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int userPostId, CancellationToken cancellationToken);
}
