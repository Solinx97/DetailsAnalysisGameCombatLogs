using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityUserApiClient
{
    Task<CommunityUserResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityUserResponse> GetByCommunityIdAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityUserResponse> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken);

    Task<bool> CanJoinAsync(Guid appUserId, int communityId, CancellationToken cancellationToken);

    Task<CommunityUserModel> CreateAsync(CommunityUserModel discussion, CancellationToken cancellationToken);

    Task LeaveAsync(Guid appUserId, int communityId, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, int communityId, CancellationToken cancellationToken);
}
