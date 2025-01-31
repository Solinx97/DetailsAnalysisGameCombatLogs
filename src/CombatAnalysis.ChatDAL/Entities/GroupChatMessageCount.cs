using CombatAnalysis.ChatDAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatMessageCount : IChatEntity
{
    [Key]
    public int Id { get; set; }

    public int Count { get; set; }

    [ForeignKey(nameof(GroupChat))]
    public int ChatId { get; set; }

    [ForeignKey(nameof(GroupChatUser))]
    public string GroupChatUserId { get; set; }
}