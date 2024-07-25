namespace CombatAnalysis.CommunicationDAL.Entities.Community;

public class CommunityDiscussion
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }

    public int CommunityId { get; set; }
}
