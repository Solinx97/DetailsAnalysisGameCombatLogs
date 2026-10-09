using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record CreateGroupChatUserModel(
        string Id,
        string Username,
        int UnreadMessages,
        Guid? LastReadMessageId,
        int GroupChatId,
        Guid AppUserId,
        Guid WhoAddId
    );
