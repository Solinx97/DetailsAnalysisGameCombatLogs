namespace CombatParser.Application.DTOs;

public class UnitHealthDto
{
    public string Id { get; set; } = string.Empty;

    public string CreatorGameId { get; set; } = string.Empty;

    public long CurrentHealth { get; set; }

    public long MaxHealth { get; set; }

    public TimeSpan Time { get; set; }

    public bool IsDead { get; set; }

    public string CombatUnitId { get; set; } = string.Empty;
}
