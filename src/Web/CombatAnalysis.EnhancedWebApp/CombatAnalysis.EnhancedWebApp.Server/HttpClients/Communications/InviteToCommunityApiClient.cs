using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class InviteToCommunityApiClient(HttpClient httpClient) : IInviteToCommunityApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<InviteToCommunityModel>> GetByUserIdAsync(Guid appUserId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"InviteToCommunity/getByUserId/{appUserId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<InviteToCommunityModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task CreateAsync(InviteToCommunityModel invite, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("InviteToCommunity", JsonContent.Create(invite), cancellationToken);
    }

    public async Task UpdateAsync(int id, InviteToCommunityModel invite, CancellationToken cancellationToken)
    {
        if (id != invite.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"InviteToCommunity/{id}", JsonContent.Create(invite), cancellationToken);
    }

    public async Task AcceptRequestAsync(int id, int communityId, Guid appUserId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"InviteToCommunity/accept/{id}?communityId={communityId}&appUserId={appUserId}", cancellationToken);
    }

    public async Task DeleteAsync(int id, int communityId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"InviteToCommunity/{id}?communityId={communityId}", cancellationToken);
    }
}
