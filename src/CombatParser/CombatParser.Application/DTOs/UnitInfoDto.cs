namespace CombatParser.Application.DTOs;

public class UnitInfoDto
{
    public string Id { get; set; } = string.Empty;

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public long ResourcesRecovery { get; set; }

    public string UnitId { get; set; }
}
