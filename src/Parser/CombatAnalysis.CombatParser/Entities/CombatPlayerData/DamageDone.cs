using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities.CombatPlayerData;

public class DamageDone : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public bool IsTargetBoss { get; set; }

    public int DamageType { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public bool IsSingleTarget { get; set; }

    public bool IsPet { get; set; }

    public int CombatPlayerId { get; set; }
}
