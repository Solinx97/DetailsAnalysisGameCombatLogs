namespace CombatParser.Application.DTOs;

public class UnitPositionDto
{
    public string Id { get; set; } = string.Empty;

    public string CreatorGameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    public TimeSpan Time { get; set; }

    public string CombatUnitId { get; set; } = string.Empty;
}
