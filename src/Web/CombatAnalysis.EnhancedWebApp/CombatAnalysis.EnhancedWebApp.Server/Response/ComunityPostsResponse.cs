using CombatAnalysis.EnhancedWebApp.Server.Models.Post;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record ComunityPostsResponse(
    IEnumerable<CommunityPostModel> Posts,
    int Count
    );
