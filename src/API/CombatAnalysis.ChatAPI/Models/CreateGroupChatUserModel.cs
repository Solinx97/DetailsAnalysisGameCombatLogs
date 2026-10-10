using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record CreateGroupChatUserModel(
        [Required][StringLength(8)] string Username,
        [Range(0, int.MaxValue)] int UnreadMessages,
        Guid? LastReadMessageId,
        [Range(1, int.MaxValue)] int GroupChatId,
        [Required] Guid AppUserId,
        [Required] Guid WhoAddId
    );
