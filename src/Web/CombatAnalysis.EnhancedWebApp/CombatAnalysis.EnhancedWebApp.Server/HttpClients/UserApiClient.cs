using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.User;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class UserApiClient(HttpClient httpClient) : IUserApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<FriendModel>> GetFiendsAsync(Guid appUserId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Friend/findByUserId/{appUserId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<FriendModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The User API returned an empty response.");
    }
}
