namespace CombatAnalysis.DAL.Entities;

public class PlayerDeath
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
