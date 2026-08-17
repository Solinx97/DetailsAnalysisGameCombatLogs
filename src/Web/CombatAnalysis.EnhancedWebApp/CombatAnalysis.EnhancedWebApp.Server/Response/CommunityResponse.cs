using CombatAnalysis.EnhancedWebApp.Server.Models.Community;

namespace CombatAnalysis.EnhancedWebApp.Server.Response;

public record CommunityResponse(
    IEnumerable<CommunityModel> Communities,
    int Count
    );
