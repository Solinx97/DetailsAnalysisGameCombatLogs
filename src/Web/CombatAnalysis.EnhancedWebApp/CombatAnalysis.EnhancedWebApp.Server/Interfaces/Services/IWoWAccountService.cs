using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IWoWAccountService
{
    Task<WoWAccountCollectionItemDto[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountPetsAsync(string regionName, CancellationToken cancellationToken);
}
