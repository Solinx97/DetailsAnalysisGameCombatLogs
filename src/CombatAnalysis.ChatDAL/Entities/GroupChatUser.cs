using CombatAnalysis.ChatDAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatUser : IChatEntity
{
    [Key]
    public string Id { get; set; }

    public string Username { get; set; }

    [ForeignKey(nameof(GroupChat))]
    public int ChatId { get; set; }

    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
}
