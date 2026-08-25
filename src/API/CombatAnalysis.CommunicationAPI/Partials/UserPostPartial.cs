using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record UserPostPartial(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Content
    );