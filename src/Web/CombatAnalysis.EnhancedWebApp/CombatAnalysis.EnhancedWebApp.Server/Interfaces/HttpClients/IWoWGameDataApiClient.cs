using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWGameDataApiClient
{
    Task<RealmsResponse> GetRealmsAsync(string regionName, CancellationToken cancellationToken);

    Task<AchievementCategoriesModel> GetAchievementCategoryAsync(string regionName, CancellationToken cancellationToken);

    Task<AchievementSelectedCategoryModel> GetAchievementCategoryAsync(string regionName, int categoryId, CancellationToken cancellationToken);

    Task<SelectedAchievementModel> GetAchievementAsync(string regionName, int achievementId, CancellationToken cancellationToken);
}
