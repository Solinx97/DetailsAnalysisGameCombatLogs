namespace CombatAnalysis.Core.Models;

public class PlayerDeathModel
{
    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
