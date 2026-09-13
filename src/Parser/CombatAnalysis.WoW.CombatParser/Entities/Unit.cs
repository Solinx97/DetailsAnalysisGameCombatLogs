namespace CombatAnalysis.WoW.CombatParser.Entities;

public class Unit
{
    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public List<UnitHealth> UnitHealthes { get; set; } = [];

    public List<UnitCast> UnitCasts { get; set; } = [];

    public List<UnitPosition> UnitPositions { get; set; } = [];
}
