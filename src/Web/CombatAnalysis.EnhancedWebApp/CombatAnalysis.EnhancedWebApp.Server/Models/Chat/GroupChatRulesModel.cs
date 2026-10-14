namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record GroupChatRulesModel(
    int Id,
    int InvitePeople,
    int RemovePeople,
    int PinMessage,
    int Announcements,
    int GroupChatId
    );
