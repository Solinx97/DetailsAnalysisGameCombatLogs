namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record PersonalChatMessagePatch(
        Guid Id,
        string? Message,
        int? Status,
        int? MarkedType
    );