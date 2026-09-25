using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
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

    public async Task<CharacterReputaionsResponse> GetReputationsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/reputations?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);
        
        var result = await response.Content.ReadFromJsonAsync<CharacterReputaionsResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterMountsResponse> GetMountsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}/collections/mounts?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterMountsResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterModel> GetProfileSummaryAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.Trim().ToLower()}?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterModel>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<MythicKeystoneModel> GetMythicKeystoneAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/mythic-keystone-profile?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<MythicKeystoneModel>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterDungeonModel> GetRaidsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/encounters/raids?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterDungeonModel>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<CharacterDungeonModel> GetDungeonsAsync(string serverName, string username, string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/encounters/dungeons?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterDungeonModel>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
