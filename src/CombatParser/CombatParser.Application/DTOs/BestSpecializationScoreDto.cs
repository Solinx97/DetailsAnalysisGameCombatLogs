namespace CombatParser.Application.DTOs;

public class BestSpecializationScoreDto
{
    public int Id { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }

    public int BossId { get; set; }
}
