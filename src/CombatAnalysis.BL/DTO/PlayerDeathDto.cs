namespace CombatAnalysis.BL.DTO;

public class PlayerDeathDto
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
