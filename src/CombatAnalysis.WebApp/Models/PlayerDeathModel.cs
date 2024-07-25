namespace CombatAnalysis.WebApp.Models;

public class PlayerDeathModel
{
    public int Id { get; set; }

    public DateTimeOffset When { get; set; }

    public int CombatPlayerId { get; set; }
}
