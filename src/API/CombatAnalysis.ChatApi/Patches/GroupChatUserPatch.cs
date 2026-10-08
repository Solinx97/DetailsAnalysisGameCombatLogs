using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatUserPatch(
        [Required] string Id,
        [Required] Guid LastReadMessageId,
        [Range(0, int.MaxValue)] int? UnreadMessages
    );
