using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string DungeonName { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public double BossHealthPercentage { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public long HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageTaken { get; set; }

    [Range(0, int.MaxValue)]
    public int ResourcesRecovery { get; set; }

    [Required]
    public bool IsWin { get; set; }

    [Required]
    public DateTimeOffset StartDate { get; set; }

    [Required]
    public DateTimeOffset FinishDate { get; set; }

    [Required]
    public List<CombatPlayerModel> CombatPlayers { get; set; } = [];

    [Required]
    public Dictionary<string, List<string>> PetsId { get; set; } = [];

    [Required]
    public BossModel Boss { get; set; } = new();

    [Required]
    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    [Required]
    public bool IsReady { get; set; }

    [Range(1, int.MaxValue)]
    public int CombatLogId { get; set; }

    public IReadOnlyCollection<CombatAuraModel> CombatAuras { get; set; } = [];
}
