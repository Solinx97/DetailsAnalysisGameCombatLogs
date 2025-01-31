using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class UnreadGroupChatMessage
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(GroupChatUser))]
    public string GroupChatUserId { get; set; }

    [ForeignKey(nameof(GroupChatMessage))]
    public int GroupChatMessageId { get; set; }
}
