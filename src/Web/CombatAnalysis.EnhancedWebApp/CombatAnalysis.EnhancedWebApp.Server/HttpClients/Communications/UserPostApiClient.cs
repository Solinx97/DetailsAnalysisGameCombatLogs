using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class UserPostApiClient(HttpClient httpClient) : IUserPostApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<UserPostsResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"UserPost/getByUserId/{appUserId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostsResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<UserPostModel> CreateAsync(UserPostModel pos, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("UserPost", JsonContent.Create(pos), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"UserPost/{id}", cancellationToken);
    }
}
