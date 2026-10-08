namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record GroupChatMessagePatch(
        Guid Id,
        string? Message,
        int? Status,
        int? MarkedType
    );