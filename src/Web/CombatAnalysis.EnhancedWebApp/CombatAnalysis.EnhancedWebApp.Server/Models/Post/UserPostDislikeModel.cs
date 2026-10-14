namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record UserPostDislikeModel(
    int Id,
    int UserPostId,
    Guid AppUserId,
    int Status
    );
