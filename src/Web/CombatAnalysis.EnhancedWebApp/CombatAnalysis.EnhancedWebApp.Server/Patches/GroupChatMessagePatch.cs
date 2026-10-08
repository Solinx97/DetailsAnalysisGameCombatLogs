namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record GroupChatMessagePatch(
        string Id,
        string? Message,
        int? Status,
        int? MarkedType
    );