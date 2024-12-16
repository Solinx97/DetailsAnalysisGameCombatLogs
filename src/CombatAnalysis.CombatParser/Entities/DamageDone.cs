using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class DamageDone : ICombatPlayerEntity
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int DamageType { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public bool IsPet { get; set; }

    public int CombatPlayerId { get; set; }
}
