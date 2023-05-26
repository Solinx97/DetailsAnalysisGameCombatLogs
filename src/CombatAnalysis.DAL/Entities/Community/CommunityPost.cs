namespace CombatAnalysis.DAL.Entities.Community;

public class CommunityPost
{
    public int Id { get; set; }

    public string Content { get; set; }

    public string OwnerId { get; set; }

    public int CommunityId { get; set; }
}
