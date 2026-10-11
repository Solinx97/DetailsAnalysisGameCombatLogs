using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record PersonalChatMessageReadPatch(
        [Required] Guid Id,
        [Range(1, int.MaxValue)] int ChatId,
        [Required] Guid ReaderId
    );
