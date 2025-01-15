using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChat
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    [ForeignKey("AppUser")]
    public string AppUserId { get; set; }
}
