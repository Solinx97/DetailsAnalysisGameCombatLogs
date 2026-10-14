namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public class CommunityUserModel
{
    public Guid Id { get; set; }

    public int CommunityId { get; set; }

    public Guid AppUserId { get; set; }

    public string? Username { get; set; }
}
