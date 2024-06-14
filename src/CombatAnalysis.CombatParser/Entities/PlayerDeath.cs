namespace CombatAnalysis.CombatParser.Entities;

public class PlayerDeath
{
    public string Username { get; set; }

    public string Skill { get; set; } = string.Empty;

    public int Value { get; set; }

    public int CurrentHP { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
