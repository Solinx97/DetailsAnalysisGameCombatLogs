using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface IUserFeedRepository
{
    Task<int> CountNewPostsAsync(UserId appUserIds, List<UserId> friendsId, DateTimeOffset lastCheck, CancellationToken cancellationToken);

    Task<(IEnumerable<UserFeedReadModel>, int)> GetUserFeedAsync(UserId appUserId, List<UserId> friendsId, int page, int pageSize, CancellationToken cancellationToken);
}
