namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record CommunityPostModel(
    int Id,
    string Content,
    string? CommunityName,
    int PostType,
    int PublicType,
    int Restrictions,
    string? Tags,
    DateTimeOffset CreatedAt,
    int CommunityId,
    Guid AppUserId,
    int LikeCount,
    int DislikeCount,
    int CommentCount,
    int Reaction
    );
