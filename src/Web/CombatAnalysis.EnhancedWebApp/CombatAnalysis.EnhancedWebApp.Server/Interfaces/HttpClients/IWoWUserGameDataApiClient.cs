using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWUserGameDataApiClient
{
    Task<AccountMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);
}
