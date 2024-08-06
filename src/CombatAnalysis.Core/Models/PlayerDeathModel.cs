namespace CombatAnalysis.Core.Models;

public class PlayerDeathModel
{
    public DateTimeOffset Date { get; set; }

    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public int CombatPlayerId { get; set; }
}
