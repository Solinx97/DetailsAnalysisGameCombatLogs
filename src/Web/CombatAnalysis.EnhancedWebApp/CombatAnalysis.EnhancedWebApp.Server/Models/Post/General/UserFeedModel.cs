namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post.General;

public record UserFeedModel(
    int Id,
    string Owner,
    string Content,
    int PublicType,
    string Tags,
    DateTimeOffset CreatedAt,
    string AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    int Reaction,
    string? CommunityName,
    int? PostType,
    int? Restrictions,
    int? CommunityId
    );
