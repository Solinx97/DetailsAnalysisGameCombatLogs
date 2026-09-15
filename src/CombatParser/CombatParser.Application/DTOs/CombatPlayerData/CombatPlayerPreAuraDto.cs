namespace CombatParser.Application.DTOs.CombatPlayerData;

public class CombatPlayerPreAuraDto
{
    public string Id { get; set; }

    public int GameId { get; set; }

    public string TargetGameId { get; set; } = string.Empty;

    public int AbilityType { get; set; }

    public int Status { get; set; }

    public string? UnitId { get; set; }
}
