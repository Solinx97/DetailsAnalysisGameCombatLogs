namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record CommunityPostLikeModel(
    int Id,
    DateTimeOffset CreatedAt,
    int CommunityId,
    int CommunityPostId,
    Guid AppUserId,
    int Status
    );
