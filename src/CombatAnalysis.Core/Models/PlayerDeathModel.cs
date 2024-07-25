namespace CombatAnalysis.Core.Models;

public class PlayerDeathModel
{
    public string Username { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
