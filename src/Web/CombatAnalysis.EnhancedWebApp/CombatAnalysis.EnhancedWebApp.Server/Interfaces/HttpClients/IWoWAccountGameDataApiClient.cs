using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWAccountGameDataApiClient
{
    Task<WoWAccountRespone> GetCharactersAsync(string regionName, CancellationToken cancellationToken);

    Task<AccountMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);
}
