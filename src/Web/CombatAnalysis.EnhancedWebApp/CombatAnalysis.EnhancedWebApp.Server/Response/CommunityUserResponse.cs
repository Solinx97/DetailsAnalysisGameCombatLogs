using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record CommunityUserResponse(
    IEnumerable<CommunityUserModel> Users,
    int Count
    );
