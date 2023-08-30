namespace CombatAnalysis.ChatApi.Models.Community;

public class InviteToCommunityModel
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToCustomerId { get; set; }

    public DateTimeOffset When { get; set; }

    public string CustomerId { get; set; }
}
