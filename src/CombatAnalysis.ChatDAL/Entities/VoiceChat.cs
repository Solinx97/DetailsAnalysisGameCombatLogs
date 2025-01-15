using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class VoiceChat
{
    [Key]
    public string Id { get; set; }

    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
}