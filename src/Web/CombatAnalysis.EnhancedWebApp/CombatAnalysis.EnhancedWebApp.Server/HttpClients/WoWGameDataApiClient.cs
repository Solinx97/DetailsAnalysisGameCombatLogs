using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Decor;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

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

    public async Task<SearchItemResponse> SearchItemAsync(string regionName, string name, string orderBy, int page, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/search/item?namespace=static-{regionName}&name.{WoWDataLocale.Locale}={name}&orderby={orderBy}:desc&_page={page}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SearchItemResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/mount/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWMountsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedWoWAccountCollectionItemModel> GetMountAsync(string regionName, int mountId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/mount/{mountId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedWoWAccountCollectionItemModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetPetsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/pet/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWPetsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedWoWAccountCollectionItemModel> GetPetAsync(string regionName, int petId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/pet/{petId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedWoWAccountCollectionItemModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetToysAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/toy/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWToysResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedWoWAccountToyItemModel> GetToyAsync(string regionName, int toyId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/toy/{toyId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedWoWAccountToyItemModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetSetsTransmogsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/item-appearance/set/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWSetTransmogsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetSlotTransmogsAsync(string regionName, string slotType, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/item-appearance/slot/{slotType}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWSlotTransmogsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedWoWAccountCollectionItemModel> GetTransmogAsync(string regionName, int transmogId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/item-appearance/{transmogId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedWoWAccountCollectionItemModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<IWoWCollectionResponse> GetDecorsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/decor/index?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWDecorsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<SelectedWoWAccountCollectionItemModel> GetDecorAsync(string regionName, int decorId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/decor/{decorId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<SelectedWoWAccountCollectionItemModel>(cancellationToken);
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

    public async Task<MythicKeystoneLeaderboardModel> GetMythicKeystoneLeaderboardAsync(int connectedRealmId, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/connected-realm/{connectedRealmId}/mythic-leaderboard/index?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<MythicKeystoneLeaderboardModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<MythicKeystoneDungeonLeaderboardModel> GetMythicDungeonKeystoneLeaderboardAsync(int connectedRealmId, long dungeonId, int periodId, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/connected-realm/{connectedRealmId}/mythic-leaderboard/{dungeonId}/period/{periodId}?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<MythicKeystoneDungeonLeaderboardModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<WoWTokenModel> GetWoWTokenAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/token/index?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWTokenModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<AuctionResponse> GetAuctionHouseCommoditiesAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"data/wow/auctions/commodities?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<AuctionResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
