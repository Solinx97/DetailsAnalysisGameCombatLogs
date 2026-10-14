using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityUserApiClient(HttpClient httpClient) : ICommunityUserApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CommunityUserResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityUser/getByUserId/{appUserId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityUserResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityUserResponse> GetByCommunityIdAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityUser/getByCommunityId/{communityId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityUserResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityUserResponse> GetShortListAsync(int communityId, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityUser/getByCommunityId/{communityId}?page=1&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityUserResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<bool> CanJoinAsync(Guid appUserId, int communityId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityUser/canJoin/{appUserId}?communityId={communityId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
        return messages;
    }

    public async Task<CommunityUserModel> CreateAsync(CommunityUserModel discussion, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityUser", JsonContent.Create(discussion), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityUserModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task LeaveAsync(Guid appUserId, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityUser/leave/{appUserId}?communityId={communityId}", cancellationToken);
    }

    public async Task DeleteAsync(Guid id, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityUser/{id}?communityId={communityId}", cancellationToken);
    }
}
