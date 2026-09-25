using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWUserGameDataApiClient
{
    Task<CharacterMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);
}
