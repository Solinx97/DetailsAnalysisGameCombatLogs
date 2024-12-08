namespace CombatAnalysis.CombatParser.Entities;

public class PlayerDeath : CombatDataBase
{
    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }
}
