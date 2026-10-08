using Chat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record CreateGroupChatModel(
    [Required][StringLength(GroupChat.NAME_MAX_LENGTH)] string Name,
    [Required] string OwnerUsername,
    [Range(0, int.MaxValue)] int InvitePeopleRule,
    [Range(0, int.MaxValue)] int RemovePeopleRule,
    [Range(0, int.MaxValue)] int PinMessageRule,
    [Range(0, int.MaxValue)] int AnnouncementsRule,
    [Required] Guid OwnerId
    );
