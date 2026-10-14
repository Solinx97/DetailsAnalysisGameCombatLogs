namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public record CommunityModel(
    int Id,
    string Name,
    string Description,
    int PolicyType,
    Guid AppUserId
    );
