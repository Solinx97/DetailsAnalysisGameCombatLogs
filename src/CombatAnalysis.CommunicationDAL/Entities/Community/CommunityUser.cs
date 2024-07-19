namespace CombatAnalysis.CommunicationDAL.Entities.Community;

public class CommunityUser
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string AppUserId { get; set; }

    public int CommunityId { get; set; }
}
