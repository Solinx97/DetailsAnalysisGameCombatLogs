namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record CommunityDiscussionCommentPatch(
    int Id,
    int CommunityDiscussionId,
    string Content
    );
