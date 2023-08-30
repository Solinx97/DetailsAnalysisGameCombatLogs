namespace CombatAnalysis.BL.DTO.Community;

public class InviteToCommunityDto
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToCustomerId { get; set; }

    public DateTimeOffset When { get; set; }

    public string CustomerId { get; set; }
}
