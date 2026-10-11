namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record GroupChatMessageReadPatch(
        Guid Id,
        int ChatId,
        Guid ReaderId
    );
