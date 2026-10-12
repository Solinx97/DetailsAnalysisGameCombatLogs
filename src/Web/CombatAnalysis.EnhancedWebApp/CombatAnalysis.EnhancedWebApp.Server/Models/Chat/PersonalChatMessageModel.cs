namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record PersonalChatMessageModel(
    Guid Id,
    string Username,
    string Message,
    string Time,
    int Status,
    int Type,
    int MarkedType,
    bool IsEdited,
    int PersonalChatId,
    Guid AppUserId
    );