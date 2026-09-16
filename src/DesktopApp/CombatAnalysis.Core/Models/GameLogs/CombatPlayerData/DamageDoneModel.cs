using CombatAnalysis.Core.Interfaces.Entities;

namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class DamageDoneModel : IDetailsEntity
{
    public string Id { get; set; } = string.Empty;

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public UnitModel Unit { get; set; } = new();

    public UnitModel Target { get; set; } = new();

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Overkill { get; set; }

    public int Mitigated { get; set; }

    public string? UnitId { get; set; }
}
