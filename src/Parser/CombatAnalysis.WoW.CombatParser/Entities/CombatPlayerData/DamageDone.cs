using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class DamageDone : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string CreatorGameId { get; set; }

    public string TargetGameId { get; set; }

    public string TargetHash { get; set; }

    public long TargetCurrentHealth { get; set; }

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Overkill { get; set; }

    public int Mitigated { get; set; }

    public int CombatPlayerId { get; set; }
}
