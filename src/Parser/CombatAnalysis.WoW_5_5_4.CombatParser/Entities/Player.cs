namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities;

public class Player
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public int Faction { get; set; }
}
