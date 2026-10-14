using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityPostApiClient(HttpClient httpClient) : ICommunityPostApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ComunityPostsResponse> GetByCommunityIdAsync(int communityId, Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityPost/getByCommunityId/{communityId}?appUserId={appUserId}&page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<ComunityPostsResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<int> CountNewPostsAsync(int communityId, DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var lastCheckValue = Uri.EscapeDataString(lastCheck.ToString("O"));
        var response = await _httpClient.GetAsync($"CommunityPost/countNewPosts/{communityId}?lastCheck={lastCheckValue}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<CommunityPostModel> CreateAsync(CommunityPostModel pos, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(pos), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityPostModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityPost/{id}", cancellationToken);
    }
}
