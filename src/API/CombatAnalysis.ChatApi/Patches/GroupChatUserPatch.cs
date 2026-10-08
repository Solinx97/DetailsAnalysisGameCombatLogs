using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatUserPatch(
        [Required] Guid Id,
        [Required] Guid LastReadMessageId,
        [Range(0, int.MaxValue)] int? UnreadMessages
    );
