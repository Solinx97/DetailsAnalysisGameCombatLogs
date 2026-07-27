namespace CombatAnalysis.CombatParser.Entities;

public class CombatUnit
{
    public string GameId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }
}
