using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IWoWAccountService
{
    Task<WoWAccountResponseDto> GetCharactersAsync(string regionName, CancellationToken cancellationToken);

    Task<CharacterModel[]> GetCharactersListAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountPetsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountToysAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountCollectionItemDto[]> GetAccountSetTransmogsAsync(string regionName, CancellationToken cancellationToken);

    Task<Dictionary<string, WoWAccountCollectionItemDto[]>> GetAccountSlotTransmogsAsync(string regionName, CancellationToken cancellationToken);

    Task<AccountDetailsDto> GetAccountDashboardAsync(string regionName, string serverName, string characterName, CancellationToken cancellationToken);
}
