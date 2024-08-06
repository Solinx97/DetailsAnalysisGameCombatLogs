namespace CombatAnalysis.CombatParserAPI.Models;

public class PlayerDeathModel
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; }

    public int LastHitValue { get; set; }

    public int CombatPlayerId { get; set; }
}
