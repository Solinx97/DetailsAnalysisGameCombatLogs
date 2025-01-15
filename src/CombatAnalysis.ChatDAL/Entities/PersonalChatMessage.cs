using CombatAnalysis.ChatDAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class PersonalChatMessage : IChatEntity
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    [ForeignKey(nameof(PersonalChat))]
    public int ChatId { get; set; }

    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
}
