namespace CombatAnalysis.CombatParser.Entities;

public class ResourceRecovery : CombatDataBase
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }
}
