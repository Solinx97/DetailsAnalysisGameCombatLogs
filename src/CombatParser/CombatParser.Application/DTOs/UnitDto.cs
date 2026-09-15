using CombatParser.Application.DTOs.CombatPlayerData;

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

    public IReadOnlyList<UnitHealthDto> UnitHealthes { get; set; } = [];

    public IReadOnlyList<UnitCastDto> UnitCasts { get; init; } = [];

    public IReadOnlyList<UnitPositionDto> UnitPositions { get; init; } = [];

    public IReadOnlyList<UnitPreAuraDto> PreAuras { get; set; } = [];

    public IReadOnlyList<UnitAuraDto> Auras { get; set; } = [];
}
