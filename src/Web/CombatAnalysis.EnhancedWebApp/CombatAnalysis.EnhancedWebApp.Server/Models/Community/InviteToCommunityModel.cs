namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class InviteToCommunityModel
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public Guid ToAppUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public Guid AppUserId { get; set; }
}
