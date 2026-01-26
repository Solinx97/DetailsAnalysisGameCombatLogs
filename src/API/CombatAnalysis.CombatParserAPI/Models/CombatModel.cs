using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; init; }

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

    [Required]
    public Dictionary<string, List<string>> PetsId { get; init; } = [];

    [Required]
    public BossModel Boss { get; private set; } = new();

    [Required]
    public string Duration { get; init; } = string.Empty;

    public bool IsReady { get; init; }

    [Range(1, int.MaxValue)]
    public int CombatLogId { get; init; }

    public IReadOnlyCollection<CombatAuraModel> CombatAuras { get; init; } = [];

    public void UpdateBoss(BossModel boss)
    {
        Boss = boss;
    }
}
