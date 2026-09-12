namespace CombatAnalysis.WoW.CombatParser.Entities;

public class CombatUnit
{
    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public List<UnitCast> UnitCasts { get; set; } = [];

    public List<UnitPosition> UnitPositions { get; set; } = [];
}
