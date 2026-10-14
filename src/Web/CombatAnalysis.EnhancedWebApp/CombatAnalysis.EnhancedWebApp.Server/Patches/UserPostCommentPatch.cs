namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record UserPostCommentPatch(
    int Id,
    int UserPostId,
    string Content
    );
