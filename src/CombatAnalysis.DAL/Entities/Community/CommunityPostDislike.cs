namespace CombatAnalysis.DAL.Entities.Community;

public class CommunityPostDislike
{
    public int Id { get; set; }

    public int CommunityPostId { get; set; }

    public string OwnerId { get; set; }
}
