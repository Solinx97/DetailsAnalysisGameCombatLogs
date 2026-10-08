using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public class PersonalChatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public Guid InitiatorId { get; set; }

    [Range(0, int.MaxValue)]
    public int InitiatorUnreadMessages { get; set; }

    [Required]
    public Guid CompanionId { get; set; }

    [Range(0, int.MaxValue)]
    public int CompanionUnreadMessages { get; set; }
}
