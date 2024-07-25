namespace CombatAnalysis.WebApp.Models.Community;

public class CommunityDiscussionCommentModel
{
    public int Id { get; set; }

    public string Content { get; set; }

    public string When { get; set; }

    public string AppUserId { get; set; }

    public int CommunityDiscussionId { get; set; }
}
