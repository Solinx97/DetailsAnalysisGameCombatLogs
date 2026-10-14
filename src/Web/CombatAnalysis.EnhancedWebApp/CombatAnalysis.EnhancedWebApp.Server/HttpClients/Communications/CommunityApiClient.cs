using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityApiClient(HttpClient httpClient) : ICommunityApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CommunityResponse> GetAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Community?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Community/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Community/getByUserId/{appUserId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityModel> CreateAsync(CommunityModel community, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("Community", JsonContent.Create(community), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task UpdateAsync(int id, CommunityPatch community, CancellationToken cancellationToken)
    {
        if (id != community.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"Community/{id}", JsonContent.Create(community), cancellationToken);
    }

    public async Task UpdatePolicyAsync(int id, CommunityRulesPatch rules, CancellationToken cancellationToken)
    {
        if (id != rules.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"Community/updateRules/{id}", JsonContent.Create(rules), cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"Community/{id}", cancellationToken);
    }
}
