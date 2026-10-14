using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityDiscussionCommentApiClient(HttpClient httpClient) : ICommunityDiscussionCommentApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<DiscussionCommentResponse> GetAsync(int discussionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityDiscussionComment/getByDiscussionId/{discussionId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<DiscussionCommentResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityDiscussionCommentModel> CreateAsync(CommunityDiscussionCommentModel discussion, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityDiscussionComment", JsonContent.Create(discussion), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityDiscussionCommentModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task UpdateAsync(int id, CommunityDiscussionCommentPatch discussion, CancellationToken cancellationToken)
    {
        if (id != discussion.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"CommunityDiscussionComment/{id}", JsonContent.Create(discussion), cancellationToken);
    }

    public async Task DeleteAsync(int id, int discussionId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityDiscussionComment/{id}?discussionId={discussionId}", cancellationToken);
    }
}
