namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public record CommunityDiscussionModel(
    int Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAt,
    Guid AppUserId,
    int CommunityId
    );
