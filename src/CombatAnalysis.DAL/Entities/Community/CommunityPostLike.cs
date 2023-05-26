namespace CombatAnalysis.DAL.Entities.Community;

public class CommunityPostLike
{
    public int Id { get; set; }

    public int CommunityPostId { get; set; }

    public string OwnerId { get; set; }
}
