using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class CommunityPostReactionsApiClient(HttpClient httpClient) : ICommunityPostReactionsApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CommunityPostCommentResponse> GetCommentsAsync(int communityPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityPostComment/getByCommunityPostId/{communityPostId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityPostCommentResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<int> CountCommentsAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityPostComment/count/{communityPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<CommunityPostCommentModel> CreateCommentAsync(CommunityPostCommentModel comment, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityPostComment", JsonContent.Create(comment), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityPostCommentModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task UpdateCommentAsync(int id, CommunityPostCommentPatch comment, CancellationToken cancellationToken)
    {
        if (id != comment.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"CommunityPostComment/{id}", JsonContent.Create(comment), cancellationToken);
    }

    public async Task DeleteCommentAsync(int id, int communityPostId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"CommunityPostComment/{id}?communityPostId={communityPostId}", cancellationToken);
    }

    public async Task<int> CountDislikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityPostDislike/count/{communityPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<int> CountLikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CommunityPostLike/count/{communityPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<CommunityPostDislikeModel> CreateDislikeAsync(CommunityPostDislikeModel dislike, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityPostDislike", JsonContent.Create(dislike), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityPostDislikeModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<CommunityPostLikeModel> CreateLikeAsync(CommunityPostLikeModel like, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("CommunityPostLike", JsonContent.Create(like), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<CommunityPostLikeModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }
}
