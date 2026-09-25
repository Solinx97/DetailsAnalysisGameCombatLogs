using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class AchievementService(IWoWGameDataApiClient httpClient, IWoWCharacterGameDataApiClient characterHttpClient) : IAchievementService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWCharacterGameDataApiClient _characterHttpClient = characterHttpClient;

    public async Task<AchievementCategoriesModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var characterAchievements = await _characterHttpClient.GetAchievementsAsync(serverName, username, regionName, cancellationToken);
        var achievementCategory = await _httpClient.GetAchievementCategoryAsync(regionName, cancellationToken);

        achievementCategory.TotalQuantity = characterAchievements.TotalQuantity;
        achievementCategory.TotalPoints = characterAchievements.TotalPoints;

        foreach (var characterCategory in characterAchievements.CategoryProgress)
        {
            var category = achievementCategory.RootCategories.FirstOrDefault(x => x.Id == characterCategory.Category.Id);
            if (category == null)
            {
                continue;
            }

            category.Quantity = characterCategory.Quantity;
            category.Points = characterCategory.Points;
        }

        return achievementCategory;
    }

    public async Task<AchievementSelectedCategoryModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, int categoryId, CancellationToken cancellationToken)
    {
        var characterAchievements = await _characterHttpClient.GetAchievementsAsync(serverName, username, regionName, cancellationToken);
        var achievementCategory = await _httpClient.GetAchievementCategoryAsync(regionName, categoryId, cancellationToken);

        if (achievementCategory.Subcategories != null)
        {
            ApplyCategoryProgress(characterAchievements, achievementCategory);
        }

        if (achievementCategory.Achievements != null)
        {
            ApplyAchievementsProgress(characterAchievements, achievementCategory);
        }

        return achievementCategory;
    }

    private static void ApplyCategoryProgress(CharacterAchievementsModel characterProgress, AchievementSelectedCategoryModel selectedCategory)
    {
        foreach (var characterCategory in characterProgress.CategoryProgress)
        {
            var category = selectedCategory.Subcategories.FirstOrDefault(x => x.Id == characterCategory.Category.Id);
            if (category == null)
            {
                continue;
            }

            category.Quantity = characterCategory.Quantity;
            category.Points = characterCategory.Points;
        }
    }

    private static void ApplyAchievementsProgress(CharacterAchievementsModel characterProgress, AchievementSelectedCategoryModel selectedCategory)
    {
        foreach (var characterAchiev in characterProgress.Achievements)
        {
            var achiev = selectedCategory.Achievements.FirstOrDefault(x => x.Id == characterAchiev.Achievement.Id);
            if (achiev == null)
            {
                continue;
            }

            achiev.CompletedTime = characterAchiev.CompletedTimestamp > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(characterAchiev.CompletedTimestamp) : null;
        }
    }
}
