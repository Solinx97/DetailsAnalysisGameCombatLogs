namespace CombatAnalysis.CombatParser.Entities;

public class HealDoneGeneral : CombatDataBase
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public double HealPerSecond { get; set; }

    public int CritNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }
}
