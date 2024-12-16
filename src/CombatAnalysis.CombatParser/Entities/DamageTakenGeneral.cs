using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class DamageTakenGeneral : ICombatPlayerEntity
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public int ActualValue { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public int CritNumber { get; set; }

    public int MissNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
