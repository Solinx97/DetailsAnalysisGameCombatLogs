using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Aggregates;

public class BestSpecializationScore
{
    private BestSpecializationScore() { }

    public BestSpecializationScore(int id, int damageDone, int healDone, DateTimeOffset? updated, int specializationId, int bossId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specializationId, nameof(specializationId));

        Id = id;
        DamageDone = damageDone;
        HealDone = healDone;
        Updated = updated;
        SpecializationId = specializationId;
        BossId = bossId;
    }

    public int Id { get; private set; }

    public int DamageDone { get; private set; }

    public int HealDone { get; private set; }

    public DateTimeOffset? Updated { get; private set; }

    public Specialization Specialization { get; private set; }

    public int SpecializationId { get; private set; }

    public Boss Boss { get; private set; }

    public int BossId { get; private set; }

    public void Update(int damageDone, int healDone)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));

        if (damageDone != DamageDone)
        {
            DamageDone = damageDone;
            Updated = DateTimeOffset.UtcNow;
        }

        if (healDone != HealDone)
        {
            HealDone = healDone;
            Updated = DateTimeOffset.UtcNow;
        }
    }
}
