namespace CombatAnalysis.WebApp.Models.Community;

public class CommunityDiscussionModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string When { get; set; }

    public string AppUserId { get; set; }

    public int CommunityId { get; set; }
}
