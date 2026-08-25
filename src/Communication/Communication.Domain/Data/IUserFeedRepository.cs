using Communication.Domain.ReadModel;

namespace Communication.Domain.Data;

public interface IUserFeedRepository
{
    Task<int> CountNewPostsAsync(string appUserIds, List<string> friendsId, DateTimeOffset lastCheck, CancellationToken cancellationToken);

    Task<(IEnumerable<UserFeedReadModel>, int)> GetUserFeedAsync(string appUserId, List<string> friendsId, int page, int pageSize, CancellationToken cancellationToken);
}
