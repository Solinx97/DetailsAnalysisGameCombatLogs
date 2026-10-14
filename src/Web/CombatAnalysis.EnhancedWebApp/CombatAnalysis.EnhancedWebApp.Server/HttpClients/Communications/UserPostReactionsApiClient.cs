using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients.Communications;

internal class UserPostReactionsApiClient(HttpClient httpClient) : IUserPostReactionsApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<UserPostCommentResponse> GetCommentsAsync(int userPostId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"UserPostComment/getByUserPostId/{userPostId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostCommentResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<int> CountCommentsAsync(int userPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"UserPostComment/count/{userPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<UserPostCommentModel> CreateCommentAsync(UserPostCommentModel comment, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("UserPostComment", JsonContent.Create(comment), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostCommentModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task UpdateCommentAsync(int id, UserPostCommentPatch comment, CancellationToken cancellationToken)
    {
        if (id != comment.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"UserPostComment/{id}", JsonContent.Create(comment), cancellationToken);
    }

    public async Task DeleteCommentAsync(int id, int userPostId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"UserPostComment/{id}?userPostId={userPostId}", cancellationToken);
    }

    public async Task<int> CountDislikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"UserPostDislike/count/{communityPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<int> CountLikeAsync(int communityPostId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"UserPostLike/count/{communityPostId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return messages;
    }

    public async Task<UserPostDislikeModel> CreateDislikeAsync(UserPostDislikeModel dislike, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("UserPostDislike", JsonContent.Create(dislike), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostDislikeModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }

    public async Task<UserPostLikeModel> CreateLikeAsync(UserPostLikeModel like, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("UserPostLike", JsonContent.Create(like), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<UserPostLikeModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Communication API returned an empty response.");
    }
}
