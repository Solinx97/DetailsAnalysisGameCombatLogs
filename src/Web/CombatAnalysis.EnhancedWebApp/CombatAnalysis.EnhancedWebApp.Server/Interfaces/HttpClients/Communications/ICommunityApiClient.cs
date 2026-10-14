using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityApiClient
{
    Task<CommunityResponse> GetAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityModel> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<CommunityResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityModel> CreateAsync(CommunityModel community, CancellationToken cancellationToken);

    Task UpdateAsync(int id, CommunityPatch community, CancellationToken cancellationToken);

    Task UpdatePolicyAsync(int id, CommunityRulesPatch rules, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
