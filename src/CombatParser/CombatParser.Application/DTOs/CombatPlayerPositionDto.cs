namespace CombatParser.Application.DTOs;

public class CombatPlayerPositionDto
{
    public string Id { get; set; } = string.Empty;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }

    public int CombatPlayerId { get; set; }
}
