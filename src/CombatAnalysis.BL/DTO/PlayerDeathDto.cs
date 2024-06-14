namespace CombatAnalysis.BL.DTO;

public class PlayerDeathDto
{
    public int Id { get; set; }

    public string Skill { get; set; }

    public int Value { get; set; }

    public int CurrentHP { get; set; }

    public DateTimeOffset Date { get; set; }

    public int CombatPlayerId { get; set; }
}
