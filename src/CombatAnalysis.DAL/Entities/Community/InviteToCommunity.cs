namespace CombatAnalysis.DAL.Entities.Community;

public class InviteToCommunity
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToCustomerId { get; set; }

    public DateTimeOffset When { get; set; }

    public int Result { get; set; }

    public string OwnerId { get; set; }

}
