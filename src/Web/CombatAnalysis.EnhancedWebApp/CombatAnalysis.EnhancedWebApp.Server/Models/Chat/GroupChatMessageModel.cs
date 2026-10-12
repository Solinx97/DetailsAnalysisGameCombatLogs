namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record GroupChatMessageModel(
    Guid Id,
    string Username,
    string Message,
    string Time,
    int Status,
    int Type,
    int MarkedType,
    bool IsEdited,
    int GroupChatId,
    Guid GroupChatUserId
    );