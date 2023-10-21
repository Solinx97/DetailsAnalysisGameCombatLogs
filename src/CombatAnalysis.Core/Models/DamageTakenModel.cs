namespace CombatAnalysis.Core.Models;

public class DamageTakenModel
{
    public int Value { get; set; }

    public int ActualValue { get; set; }

    public TimeSpan Time { get; set; }

    public string FromEnemy { get; set; }

    public string ToPlayer { get; set; }

    public string SpellOrItem { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Mitigated { get; set; }

    public bool IsDodge { get; set; }

    public bool IsParry { get; set; }

    public bool IsMiss { get; set; }

    public bool IsResist { get; set; }

    public bool IsImmune { get; set; }

    public bool IsAbsorb { get; set; }

    public bool IsCrushing { get; set; }

    public int CombatPlayerId { get; set; }
}
