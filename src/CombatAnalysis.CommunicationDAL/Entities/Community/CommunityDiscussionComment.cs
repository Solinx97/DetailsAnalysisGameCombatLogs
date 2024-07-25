namespace CombatAnalysis.CommunicationDAL.Entities.Community;

public class CommunityDiscussionComment
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }

    public int CommunityDiscussionId { get; set; }
}
