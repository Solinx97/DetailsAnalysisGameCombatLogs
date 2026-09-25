using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWCharacterGameDataApiClient
{
    Task<CharacterAchievementsModel> GetAchievementsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<CharacterReputaionsResponse> GetReputationsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<CharacterMountsResponse> GetMountsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<CharacterModel> GetProfileSummaryAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<MythicKeystoneModel> GetMythicKeystoneAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<CharacterDungeonModel> GetRaidsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<CharacterDungeonModel> GetDungeonsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);
}
