using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IAchivmentService
{
    Task<AchievementCategoriesModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName);

    Task<AchievementSelectedCategoryModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, int categoryId);
}
