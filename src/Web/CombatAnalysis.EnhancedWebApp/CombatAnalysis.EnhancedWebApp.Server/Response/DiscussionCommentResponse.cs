using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record DiscussionCommentResponse(
    IEnumerable<CommunityDiscussionCommentModel> Comments,
    int Count
);
