namespace CombatAnalysis.CombatParser.Entities;

public class ResourceRecovery : CombatDataBase
{
    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string SpellOrItem { get; set; }
}
