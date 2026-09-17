namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatLogStatusModel
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int Status { get; private set; }

    public int CombatLogId { get; set; }
}
