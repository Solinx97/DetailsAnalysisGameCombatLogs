namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record PersonalChatMessagePatch(
        string Id,
        string? Message,
        int? Status,
        int? MarkedType
    );