using Communication.Domain.ReadModel;

namespace Communication.Domain.Data;

public interface IUserFeedRepository
{
    Task<IEnumerable<UserFeed>> GetUserFeedAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);
}
