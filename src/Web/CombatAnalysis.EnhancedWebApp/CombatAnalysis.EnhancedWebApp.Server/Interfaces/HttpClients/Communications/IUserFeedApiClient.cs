using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface IUserFeedApiClient
{
    Task<int> CountNewPostsAsync(Guid appUserId, DateTimeOffset lastCheck, CancellationToken cancellationToken);

    Task<UserFeedResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken);
}
