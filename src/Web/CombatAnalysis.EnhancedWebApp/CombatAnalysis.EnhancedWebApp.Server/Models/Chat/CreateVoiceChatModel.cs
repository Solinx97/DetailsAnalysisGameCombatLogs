namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record CreateVoiceChatModel(
    int GroupChatId,
    Guid AppUserId
    );
