using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Professions;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class WoWCharacterGameDataApiClient(HttpClient httpClient) : IWoWCharacterGameDataApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CharacterAchievementsModel> GetAchievementsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/achievements?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterAchievementsModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterAchievementStatisticsResponse> GetAchievementStatisticsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/achievements/statistics?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterAchievementStatisticsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterReputaionsResponse> GetReputationsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/reputations?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);
        
        var result = await response.Content.ReadFromJsonAsync<CharacterReputaionsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<WoWAccountMountsResponse> GetMountsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/collections/mounts?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWAccountMountsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterSummaryModel> GetProfileSummaryAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterSummaryModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterEquipmentsResponse> GetEquipmentsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/equipment?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterEquipmentsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterStatsModel> GetStatsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/statistics?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterStatsModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<MythicKeystoneModel> GetMythicKeystoneAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/mythic-keystone-profile?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<MythicKeystoneModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<MythicKeystoneSeasonModel> GetMythicKeystoneSeasonAsync(string serverName, int seasonId, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/mythic-keystone-profile/season/{seasonId}?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<MythicKeystoneSeasonModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterDungeonModel> GetRaidsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/encounters/raids?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterDungeonModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterDungeonModel> GetDungeonsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/encounters/dungeons?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterDungeonModel>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterProfessionsResponse> GetProfessionsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/professions?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterProfessionsResponse>(cancellationToken);
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
