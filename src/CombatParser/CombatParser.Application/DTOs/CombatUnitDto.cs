namespace CombatParser.Application.DTOs;

public class CombatUnitDto
{
    public string GameId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }
}
