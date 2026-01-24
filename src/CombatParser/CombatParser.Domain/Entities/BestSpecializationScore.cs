namespace CombatParser.Domain.Entities;

public class BestSpecializationScore
{
    public int Id { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public Specialization Specialization { get; set; }

    public int SpecializationId { get; set; }

    public Boss Boss { get; set; }

    public int BossId { get; set; }
}
