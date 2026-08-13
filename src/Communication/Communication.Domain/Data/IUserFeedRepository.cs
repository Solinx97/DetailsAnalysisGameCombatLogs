using Communication.Domain.ReadModel;

namespace Communication.Domain.Data;

public interface IUserFeedRepository
{
    Task<(IEnumerable<UserFeedReadModel>, int)> GetUserFeedAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);
}
