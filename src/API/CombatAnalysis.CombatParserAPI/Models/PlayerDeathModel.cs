namespace CombatAnalysis.CombatParserAPI.Models;

public class PlayerDeathModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Skill { get; set; }

    public int Value { get; set; }

    public int CurrentHP { get; set; }

    public string Date { get; set; }

    public int CombatPlayerId { get; set; }
}
