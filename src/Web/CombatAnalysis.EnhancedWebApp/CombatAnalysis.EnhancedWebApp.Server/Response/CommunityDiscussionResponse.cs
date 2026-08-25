using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record CommunityDiscussionResponse(
    IEnumerable<CommunityDiscussionModel> Discussions,
    int Count
);
