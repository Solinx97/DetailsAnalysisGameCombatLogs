using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityPostReactionsApiClient
{
    Task<CommunityPostCommentResponse> GetCommentsAsync(int communityPostId, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountCommentsAsync(int communityPostId, CancellationToken cancellationToken);

    Task<CommunityPostCommentModel> CreateCommentAsync(CommunityPostCommentModel comment, CancellationToken cancellationToken);

    Task UpdateCommentAsync(int id, CommunityPostCommentPatch comment, CancellationToken cancellationToken);

    Task DeleteCommentAsync(int id, int communityPostId, CancellationToken cancellationToken);

    Task<int> CountDislikeAsync(int communityPostId, CancellationToken cancellationToken);

    Task<int> CountLikeAsync(int communityPostId, CancellationToken cancellationToken);

    Task<CommunityPostDislikeModel> CreateDislikeAsync(CommunityPostDislikeModel dislike, CancellationToken cancellationToken);

    Task<CommunityPostLikeModel> CreateLikeAsync(CommunityPostLikeModel like, CancellationToken cancellationToken);
}
