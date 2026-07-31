using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities.CombatPlayerData;

public class HealDoneGeneral : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public double HealPerSecond { get; set; }

    public int CritNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
