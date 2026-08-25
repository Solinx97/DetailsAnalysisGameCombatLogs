namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class CommunityDiscussionCommentModel
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public string AppUserId { get; set; } = string.Empty;

    public int CommunityDiscussionId { get; set; }
}
