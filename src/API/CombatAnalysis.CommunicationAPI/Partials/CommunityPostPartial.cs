using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityPostPartial(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Content
    );
