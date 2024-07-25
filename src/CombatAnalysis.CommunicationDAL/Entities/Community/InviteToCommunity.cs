namespace CombatAnalysis.CommunicationDAL.Entities.Community;

public class InviteToCommunity
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToAppUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }
}
