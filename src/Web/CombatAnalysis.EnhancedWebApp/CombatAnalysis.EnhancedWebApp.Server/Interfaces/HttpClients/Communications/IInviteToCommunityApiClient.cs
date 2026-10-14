using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface IInviteToCommunityApiClient
{
    Task<IEnumerable<InviteToCommunityModel>> GetByUserIdAsync(Guid appUserId, CancellationToken cancellationToken);

    Task CreateAsync(InviteToCommunityModel invite, CancellationToken cancellationToken);

    Task UpdateAsync(int id, InviteToCommunityModel invite, CancellationToken cancellationToken);

    Task AcceptRequestAsync(int id, int communityId, Guid appUserId, CancellationToken cancellationToken);

    Task DeleteAsync(int id, int communityId, CancellationToken cancellationToken);
}
