namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record UserPostLikeModel(
    int Id,
    int UserPostId,
    Guid AppUserId,
    int Status
    );
