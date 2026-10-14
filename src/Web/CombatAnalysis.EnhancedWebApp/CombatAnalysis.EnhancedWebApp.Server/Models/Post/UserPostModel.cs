namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record UserPostModel(
    int Id,
    string Content,
    int PublicType,
    string? Tags,
    DateTimeOffset CreatedAt,
    Guid AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    int Reaction
    );
