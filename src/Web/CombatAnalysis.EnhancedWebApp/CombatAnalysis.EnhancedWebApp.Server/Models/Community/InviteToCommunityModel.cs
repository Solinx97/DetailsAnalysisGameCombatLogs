namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public record InviteToCommunityModel(
    int Id,
    int CommunityId,
    Guid ToAppUserId,
    DateTimeOffset When,
    Guid AppUserId
    );
