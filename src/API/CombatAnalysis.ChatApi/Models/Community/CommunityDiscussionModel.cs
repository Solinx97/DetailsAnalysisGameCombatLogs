namespace CombatAnalysis.ChatApi.Models.Community;

public class CommunityDiscussionModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string When { get; set; }

    public string CustomerId { get; set; }

    public int CommunityId { get; set; }
}
