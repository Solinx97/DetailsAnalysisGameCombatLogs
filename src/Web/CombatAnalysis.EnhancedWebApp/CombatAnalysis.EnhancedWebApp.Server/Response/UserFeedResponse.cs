using CombatAnalysis.EnhancedWebApp.Server.Models.Post.General;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record UserFeedResponse(
    IEnumerable<UserFeedModel> Posts,
    int Count
    );
