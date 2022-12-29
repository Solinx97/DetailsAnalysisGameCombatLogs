namespace CombatAnalysis.BL.DTO;

public class CombatLogDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public bool IsReady { get; set; }
}
