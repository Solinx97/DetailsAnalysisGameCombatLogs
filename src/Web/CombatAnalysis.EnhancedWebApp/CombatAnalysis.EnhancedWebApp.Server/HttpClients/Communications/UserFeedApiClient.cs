using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class UserFeedApiClient(HttpClient httpClient, IUserApiClient userHttpClient) : IUserFeedApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IUserApiClient _userHttpClient = userHttpClient;

    public async Task<int> CountNewPostsAsync(Guid appUserId, DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var friends = await _userHttpClient.GetFiendsAsync(appUserId, cancellationToken);
        var friendsIds = friends
            .Select(x =>
                x.WhoFriendId == appUserId
                    ? x.ForWhomId
                    : x.WhoFriendId)
            .ToList();

        var query = string.Join("&", friendsIds.Select(id => $"friendsIds={Uri.EscapeDataString(id.ToString())}"));
        var lastCheckValue = Uri.EscapeDataString(lastCheck.ToString("O"));
        var response = await _httpClient.GetAsync($"UserFeed/countNewPosts/{appUserId}?{query}&lastCheck={lastCheckValue}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<UserFeedResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var friends = await _userHttpClient.GetFiendsAsync(appUserId, cancellationToken);
        var friendsIds = friends
            .Select(x =>
                x.WhoFriendId == appUserId
                    ? x.ForWhomId
                    : x.WhoFriendId)
            .ToList();

        var query = string.Join("&", friendsIds.Select(id => $"friendsIds={Uri.EscapeDataString(id.ToString())}"));
        var response = await _httpClient.GetAsync($"UserFeed/{appUserId}?{query}&page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserFeedResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }
}
