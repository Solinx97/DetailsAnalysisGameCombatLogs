using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitInfoModel
{
    public string Id { get; set; } = string.Empty;

    public long ResourcesRecovery { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public long HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageTaken { get; set; }

    public string? UnitId { get; set; }
}
