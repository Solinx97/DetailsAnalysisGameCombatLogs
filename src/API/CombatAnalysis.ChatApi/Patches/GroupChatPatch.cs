using Chat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatPatch(
    [Required] int Id,
    [StringLength(GroupChat.NAME_MAX_LENGTH, MinimumLength = 1)] string Name,
    string? OwnerId
);
