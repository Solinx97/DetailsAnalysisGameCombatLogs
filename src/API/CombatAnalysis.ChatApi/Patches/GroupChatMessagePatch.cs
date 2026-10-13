using Chat.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatMessagePatch(
        [Required] Guid Id,
        string? Message,
        MessageStatus? Status,
        MessageMarkedType? MarkedType
    );