using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces;

public interface IWoWCharacterService
{
    Task<WoWAccountCollectionItemDto[]> GetDecorsAsync(string regionName, string serverName, string username, CancellationToken cancellationToken);
}
