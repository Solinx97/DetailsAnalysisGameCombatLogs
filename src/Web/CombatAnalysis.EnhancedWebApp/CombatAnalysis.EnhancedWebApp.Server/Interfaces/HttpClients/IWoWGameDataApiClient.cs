using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWGameDataApiClient
{
    Task<RealmsResponse> GetRealmsAsync(string regionName, CancellationToken cancellationToken);

    Task<MountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<SelectedMountModel> GetMountAsync(string regionName, int mountId, CancellationToken cancellationToken);

    Task<AchievementCategoriesModel> GetAchievementCategoryAsync(string regionName, CancellationToken cancellationToken);

    Task<AchievementSelectedCategoryModel> GetAchievementCategoryAsync(string regionName, int categoryId, CancellationToken cancellationToken);

    Task<SelectedAchievementModel> GetAchievementAsync(string regionName, int achievementId, CancellationToken cancellationToken);

    Task<MythicKeystoneLeaderboardModel> GetMythicKeystoneLeaderboardAsync(int connectedRealmId, string regionName, CancellationToken cancellationToken);

    Task<MythicKeystoneDungeonLeaderboardModel> GetMythicDungeonKeystoneLeaderboardAsync(int connectedRealmId, long dungeonId, int periodId, string regionName, CancellationToken cancellationToken);
}
