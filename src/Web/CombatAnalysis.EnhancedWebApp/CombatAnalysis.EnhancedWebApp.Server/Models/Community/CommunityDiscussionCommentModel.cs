namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public record CommunityDiscussionCommentModel(
    int Id,
    string Content,
    DateTimeOffset CreatedAt,
    Guid AppUserId,
    int CommunityDiscussionId
    );
