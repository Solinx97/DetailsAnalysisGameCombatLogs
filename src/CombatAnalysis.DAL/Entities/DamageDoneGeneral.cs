using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class DamageDoneGeneral : ICombatPlayerEntity
{
    public int Id { get; set; }

    public int Value { get; set; }

    public double DamagePerSecond { get; set; }

    public string SpellOrItem { get; set; }

    public int CritNumber { get; set; }

    public int MissNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public bool IsPet { get; set; }

    public int CombatPlayerId { get; set; }
}
