using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class WoWGameDataApiClient(HttpClient httpClient) : IWoWGameDataApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<RealmsResponse> GetRealmsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/realm/index?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<RealmsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<AchievementCategoriesModel> GetAchievementCategoryAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"/data/wow/achievement-category/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<AchievementCategoriesModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<AchievementSelectedCategoryModel> GetAchievementCategoryAsync(string regionName, int categoryId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/achievement-category/{categoryId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<AchievementSelectedCategoryModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedAchievementModel> GetAchievementAsync(string regionName, int achievementId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/achievement/{achievementId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedAchievementModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
