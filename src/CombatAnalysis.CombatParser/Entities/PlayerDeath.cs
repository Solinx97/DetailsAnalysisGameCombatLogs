namespace CombatAnalysis.CombatParser.Entities;

public class PlayerDeath
{
    public string Username { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
