using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IWoWAccountService
{
    Task<WoWAccountCollectionItemDto[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountPetsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountToysAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountSetTransmogsAsync(string regionName, CancellationToken cancellationToken);

    Task<Dictionary<string, WoWAccountCollectionItemDto[]>> GetAccountSlotTransmogsAsync(string regionName, CancellationToken cancellationToken);
}
