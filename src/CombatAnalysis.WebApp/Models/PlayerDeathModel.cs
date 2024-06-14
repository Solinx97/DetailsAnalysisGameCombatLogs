namespace CombatAnalysis.WebApp.Models;

public class PlayerDeathModel
{
    public int Id { get; set; }

    public string Skill { get; set; }

    public int Value { get; set; }

    public int CurrentHP { get; set; }

    public DateTimeOffset When { get; set; }

    public int CombatPlayerId { get; set; }
}
