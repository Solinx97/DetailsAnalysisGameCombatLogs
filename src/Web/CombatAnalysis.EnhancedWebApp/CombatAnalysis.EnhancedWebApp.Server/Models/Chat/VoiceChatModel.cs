namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record VoiceChatModel(
    string Id,
    int GroupChatId,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastAcrivityAt
    );
