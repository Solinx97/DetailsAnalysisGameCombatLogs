using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.PartialModels;

public record CombatLogIsReadyPatch(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int NumberReadyCombats,
    [Range(0, int.MaxValue)] int CombatsInQueue
    );
