namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatUnitModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }
}
