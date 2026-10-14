namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record CommunityPostCommentPatch(
    int Id,
    int CommunityPostId,
    string Content
    );
