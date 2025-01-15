using CombatAnalysis.ChatDAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatRules : IChatEntity
{
    [Key]
    public int Id { get; set; }

    public int InvitePeople { get; set; }

    public int RemovePeople { get; set; }

    public int PinMessage { get; set; }

    public int Announcements { get; set; }

    [ForeignKey(nameof(GroupChat))]
    public int ChatId { get; set; }
}
