using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatMessageReadPatch(
        [Required] Guid Id,
        [Range(1, int.MaxValue)] int ChatId,
        [Required] Guid ReaderId
    );
