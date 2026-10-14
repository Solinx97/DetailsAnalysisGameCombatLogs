using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityDiscussionApiClient
{
    Task<CommunityDiscussionResponse> GetAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityDiscussionModel> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<CommunityDiscussionResponse> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken);

    Task<CommunityDiscussionModel> CreateAsync(CommunityDiscussionModel discussion, CancellationToken cancellationToken);

    Task UpdateAsync(int id, CommunityDiscussionPatch discussion, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
