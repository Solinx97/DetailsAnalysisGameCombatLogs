namespace CombatParser.Application.DTOs;

public class UnitDto
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }

    public UnitInfoDto UnitInfo { get; set; }
}
