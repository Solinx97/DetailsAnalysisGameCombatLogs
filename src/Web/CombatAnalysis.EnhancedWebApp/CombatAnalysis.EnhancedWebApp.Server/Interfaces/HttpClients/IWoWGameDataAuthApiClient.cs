using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWGameDataAuthApiClient
{
    Task<BattleNetTokenResponse> GetTokenAsync(CancellationToken cancellationToken);

    Task<BattleNetTokenResponse> AuthorizationCodeExchangeAsync(string authorizationCode, CancellationToken cancellationToken);
}
