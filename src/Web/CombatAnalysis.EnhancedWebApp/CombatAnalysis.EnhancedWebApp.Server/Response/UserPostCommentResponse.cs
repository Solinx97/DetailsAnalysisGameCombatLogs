using CombatAnalysis.EnhancedWebApp.Server.Models.Post;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record UserPostCommentResponse(
    IEnumerable<UserPostCommentModel> Comments,
    int Count
);
