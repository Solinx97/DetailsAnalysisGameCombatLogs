namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public record CommunityPostCommentModel(
    int Id,
    string Content,
    int CommentType,
    DateTimeOffset CreatedAt,
    int CommunityId,
    int CommunityPostId,
    Guid AppUserId
    );
