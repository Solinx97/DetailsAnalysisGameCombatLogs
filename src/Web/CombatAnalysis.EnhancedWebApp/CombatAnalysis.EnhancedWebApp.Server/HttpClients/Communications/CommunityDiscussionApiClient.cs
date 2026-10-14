using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityDiscussionApiClient(HttpClient httpClient) : ICommunityDiscussionApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CommunityDiscussionResponse> GetAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityDiscussion/getByCommunityId/{communityId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityDiscussionResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityDiscussionModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityDiscussion/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityDiscussionModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityDiscussionResponse> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityDiscussion/getByCommunityId/{communityId}?page=1&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityDiscussionResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityDiscussionModel> CreateAsync(CommunityDiscussionModel discussion, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityDiscussion", JsonContent.Create(discussion), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityDiscussionModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task UpdateAsync(int id, CommunityDiscussionPatch discussion, CancellationToken cancellationToken)
    {
        if (id != discussion.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"CommunityDiscussion/{id}", JsonContent.Create(discussion), cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityDiscussion/{id}", cancellationToken);
    }
}
