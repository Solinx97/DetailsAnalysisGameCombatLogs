using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IMythicKeystoneService
{
    Task<MythicKeystoneLeaderboardDto> GetMythicKeystoneLeaderboardAsync(int connectedRealmId, int periodId, string regionName, CancellationToken cancellationToken);
}
