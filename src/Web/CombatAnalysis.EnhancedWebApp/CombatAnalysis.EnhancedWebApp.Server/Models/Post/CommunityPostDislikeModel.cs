namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record CommunityPostDislikeModel(
    int Id,
    DateTimeOffset CreatedAt,
    int CommunityId,
    int CommunityPostId,
    Guid AppUserId,
    int Status
    );
