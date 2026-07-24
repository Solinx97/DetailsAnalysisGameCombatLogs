namespace CombatParser.Application.DTOs;

public class UnitHeathDto
{
    public string Id { get; set; } = string.Empty;

    public string GamePlayerId { get; set; } = string.Empty;

    public int TargetCurrentHealth { get; set; }

    public int TargetMaxHealth { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }
}
