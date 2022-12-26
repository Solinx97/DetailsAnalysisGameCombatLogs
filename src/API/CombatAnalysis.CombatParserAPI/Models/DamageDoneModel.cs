namespace CombatAnalysis.CombatParserAPI.Models;

public class DamageDoneModel
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string FromPlayer { get; set; }

    public string ToEnemy { get; set; }

    public string SpellOrItem { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public bool IsDodge { get; set; }

    public bool IsParry { get; set; }

    public bool IsMiss { get; set; }

    public bool IsResist { get; set; }

    public bool IsImmune { get; set; }

    public bool IsCrit { get; set; }

    public int CombatPlayerId { get; set; }
}
