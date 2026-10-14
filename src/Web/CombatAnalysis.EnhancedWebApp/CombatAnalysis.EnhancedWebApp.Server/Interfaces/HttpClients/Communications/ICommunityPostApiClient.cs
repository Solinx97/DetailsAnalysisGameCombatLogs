using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityPostApiClient
{
    Task<ComunityPostsResponse> GetByCommunityIdAsync(int communityId, Guid appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountNewPostsAsync(int communityId, DateTimeOffset lastCheck, CancellationToken cancellationToken);

    Task<CommunityPostModel> CreateAsync(CommunityPostModel pos, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
