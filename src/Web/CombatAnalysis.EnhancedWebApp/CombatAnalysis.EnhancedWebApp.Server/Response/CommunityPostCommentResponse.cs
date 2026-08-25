using CombatAnalysis.EnhancedWebApp.Server.Models.Post;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record CommunityPostCommentResponse(
    IEnumerable<CommunityPostCommentModel> Comments,
    int Count
);
