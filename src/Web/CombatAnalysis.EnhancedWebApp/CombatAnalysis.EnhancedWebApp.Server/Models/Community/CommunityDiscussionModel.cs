namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class CommunityDiscussionModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public string AppUserId { get; set; } = string.Empty;

    public int CommunityId { get; set; }
}
