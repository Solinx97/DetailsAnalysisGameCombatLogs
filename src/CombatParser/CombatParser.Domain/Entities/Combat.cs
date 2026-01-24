using CombatParser.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Entities;

public class Combat
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string DungeonName { get; set; } = string.Empty;

    public double BossHealthPercentage { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    [NotMapped]
    [MaxLength(126)]
    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }

    public int BossId { get; set; }

    public CombatLog CombatLog { get; set; }

    public int CombatLogId { get; set; }

    public ICollection<CombatPlayer> CombatPlayers { get; set; } = [];

    public ICollection<CombatPlayerPosition> CombatPlayerPositions { get; set; } = [];

    public ICollection<CombatAura> CombatAuras { get; set; } = [];

    public ICollection<CombatTarget> CombatTargets { get; set; } = [];
}
