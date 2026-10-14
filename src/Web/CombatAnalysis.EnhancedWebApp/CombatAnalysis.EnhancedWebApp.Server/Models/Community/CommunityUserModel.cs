namespace CombatAnalysis.EnhancedWebApp.Server.Models.Community;

public record CommunityUserModel(
    Guid Id,
    int CommunityId,
    Guid AppUserId,
    string? Username
    );