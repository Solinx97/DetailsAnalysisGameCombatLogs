using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record CreateVoiceChatModel(
    [Range(1, int.MaxValue)] int GroupChatId,
    [Required] Guid AppUserId
    );
