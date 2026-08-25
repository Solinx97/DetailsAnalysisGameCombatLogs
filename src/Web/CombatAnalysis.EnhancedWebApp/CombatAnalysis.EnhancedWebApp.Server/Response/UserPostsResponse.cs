using CombatAnalysis.EnhancedWebApp.Server.Models.Post;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record UserPostsResponse(
    IEnumerable<UserPostModel> Posts,
    int Count
    );
