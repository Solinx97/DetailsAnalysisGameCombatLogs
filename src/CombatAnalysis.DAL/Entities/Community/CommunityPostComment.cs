namespace CombatAnalysis.DAL.Entities.Community;

public class CommunityPostComment
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int CommunityPostId { get; set; }

    public string OwnerId { get; set; }
}
