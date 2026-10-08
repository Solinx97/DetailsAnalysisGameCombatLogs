using Chat.Domain.Enums.GroupChatRules;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record GroupChatRulesModel(
    [Range(0, int.MaxValue)] int Id,
    [Range((int)InvitePeopleRestrictions.Anyone, (int)InvitePeopleRestrictions.Owner)] int InvitePeople,
    [Range((int)RemovePeopleRestrictions.Anyone, (int)RemovePeopleRestrictions.Owner)] int RemovePeople,
    [Range((int)PinMessageRestrictions.Anyone, (int)PinMessageRestrictions.Owner)] int PinMessage,
    [Range((int)AnnouncementsRestrictions.Anyone, (int)AnnouncementsRestrictions.Owner)] int Announcements,
    [Range(1, int.MaxValue)] int GroupChatId
    );
