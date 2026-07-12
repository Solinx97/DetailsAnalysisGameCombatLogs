namespace CombatParser.Application.DTOs;

public class CombatDto
{
    public int Id { get; set; }

    public string DungeonName { get; set; } = string.Empty;

    public double BossHealthPercentage { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public string Duration { get; set; } = string.Empty;

    public BossDto Boss { get; set; }

    public int CombatLogId { get; set; }
}
