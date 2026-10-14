using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface IUserPostReactionsApiClient
{
    Task<UserPostCommentResponse> GetCommentsAsync(int userPostId, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountCommentsAsync(int userPostId, CancellationToken cancellationToken);

    Task<UserPostCommentModel> CreateCommentAsync(UserPostCommentModel comment, CancellationToken cancellationToken);

    Task UpdateCommentAsync(int id, UserPostCommentPatch comment, CancellationToken cancellationToken);

    Task DeleteCommentAsync(int id, int userPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int communityPostId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(int communityPostId, CancellationToken cancellationToken);

    Task<UserPostDislikeModel> CreateDislikeAsync(UserPostDislikeModel dislike, CancellationToken cancellationToken);

    Task<UserPostLikeModel> CreateLikeAsync(UserPostLikeModel like, CancellationToken cancellationToken);
}
