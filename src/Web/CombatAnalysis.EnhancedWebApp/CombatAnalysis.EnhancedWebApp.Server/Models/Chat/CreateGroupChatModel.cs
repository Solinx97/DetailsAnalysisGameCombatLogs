namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record CreateGroupChatModel(
    string Name,
    string OwnerUsername,
    int InvitePeopleRule,
    int RemovePeopleRule,
    int PinMessageRule,
    int AnnouncementsRule,
    string OwnerId
    );
