using CombatAnalysis.EnhancedWebApp.Server.Models.User;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IUserApiClient
{
    Task<IEnumerable<FriendModel>> GetFiendsAsync(Guid appUserId, CancellationToken cancellationToken);
}
