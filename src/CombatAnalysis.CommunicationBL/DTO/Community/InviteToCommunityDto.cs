namespace CombatAnalysis.CommunicationBL.DTO.Community;

public class InviteToCommunityDto
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToAppUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }
}
