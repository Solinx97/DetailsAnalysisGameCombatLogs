namespace CombatAnalysis.CombatParser.Entities;

public class ResourceRecovery
{
    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string SpellOrItem { get; set; }

    public int CombatPlayerId { get; set; }
}
