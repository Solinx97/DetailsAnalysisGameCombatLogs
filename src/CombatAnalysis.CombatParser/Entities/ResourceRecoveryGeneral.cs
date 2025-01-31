using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class ResourceRecoveryGeneral : ICombatPlayerEntity
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public double ResourcePerSecond { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
