using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWAccountGameDataApiClient
{
    Task<WoWAccountRespone> GetCharactersAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountPetsResponse> GetPetsAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountToysResponse> GetToysAsync(string regionName, CancellationToken cancellationToken);

    Task<WoWAccountTransmogResponse> GetTransmogsAsync(string regionName, CancellationToken cancellationToken);
}
