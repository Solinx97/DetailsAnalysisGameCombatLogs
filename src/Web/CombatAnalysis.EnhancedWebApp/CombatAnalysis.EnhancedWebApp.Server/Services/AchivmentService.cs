using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class AchivmentService(IHttpClientHelper httpClient) : IAchivmentService
{
    private readonly IHttpClientHelper _httpClient = httpClient;

    public async Task<AchievementCategoriesModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName)
    {
        var characterAchievements = await GetCharacterAchievementsAsync(serverName, username, regionName);
        ArgumentNullException.ThrowIfNull(characterAchievements.CategoryProgress, nameof(characterAchievements.CategoryProgress));

        var responseMessage = await _httpClient.GetAsync($"/data/wow/achievement-category/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}");
        var achievementCategory = await responseMessage.Content.ReadFromJsonAsync<AchievementCategoriesModel>();
        ArgumentNullException.ThrowIfNull(achievementCategory, nameof(achievementCategory));

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

    public async Task<AchievementSelectedCategoryModel> GetCharacterAchievementCategoryAsync(string serverName, string username, string regionName, int categoryId)
    {
        var characterAchievements = await GetCharacterAchievementsAsync(serverName, username, regionName);

        var responseMessage = await _httpClient.GetAsync($"data/wow/achievement-category/{categoryId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}");
        var achievementCategory = await responseMessage.Content.ReadFromJsonAsync<AchievementSelectedCategoryModel>();
        ArgumentNullException.ThrowIfNull(achievementCategory, nameof(achievementCategory));

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

    private async Task<CharacterAchievementsModel> GetCharacterAchievementsAsync(string serverName, string username, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/achievements?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var characterAchievements = await responseMessage.Content.ReadFromJsonAsync<CharacterAchievementsModel>();
        ArgumentNullException.ThrowIfNull(characterAchievements, nameof(characterAchievements));

        return characterAchievements;
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
