namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record UserPostCommentModel(
    int Id,
    string Content,
    DateTimeOffset CreatedAt,
    int UserPostId,
    Guid AppUserId
    );
