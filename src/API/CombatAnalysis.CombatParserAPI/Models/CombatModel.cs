using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; init; }

    public int GameVersion { get; init; }

    [Required]
    public string DungeonName { get; init; } = string.Empty;

    [Range(0, int.MaxValue)]
    public double BossHealthPercentage { get; init; }

    [Range(0, int.MaxValue)]
    public long DamageDone { get; init; }

    [Range(0, int.MaxValue)]
    public long HealDone { get; init; }

    [Range(0, int.MaxValue)]
    public long DamageTaken { get; init; }

    [Range(0, int.MaxValue)]
    public int ResourcesRecovery { get; init; }

    public bool IsWin { get; init; }

    [Required]
    public DateTimeOffset StartDate { get; init; }

    [Required]
    public DateTimeOffset FinishDate { get; init; }

    [Required]
    public List<CombatPlayerModel> CombatPlayers { get; init; } = [];

    public List<CombatUnitModel> Units { get; init; } = [];

    public List<UnitCastModel> UnitCasts { get; init; } = [];

    public List<UnitHealthModel> UnitHealths { get; init; } = [];

    public List<UnitPositionModel> UnitPositions { get; init; } = [];

    [Required]
    public Dictionary<string, List<string>> PetsId { get; init; } = [];

    [Required]
    public string Duration { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CombatLogId { get; init; }

    [Required]
    public BossModel Boss { get; init; } = new();
}
