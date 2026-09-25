using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IAchievementService
{
    Task<AchievementCategoriesModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, CancellationToken cancellationToken);

    Task<AchievementSelectedCategoryModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, int categoryId, CancellationToken cancellationToken);
}
