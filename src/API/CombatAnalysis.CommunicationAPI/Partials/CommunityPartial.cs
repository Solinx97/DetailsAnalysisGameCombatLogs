using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityPartial(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Name
    );

