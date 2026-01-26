using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.PartialModels;

public record CombatLogPatch(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Name
    );
