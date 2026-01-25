namespace CombatParser.Domain.Entities;

public record SpecializationScore
{
    private SpecializationScore() { }

    public SpecializationScore(double damageScore, int damageDone, double healScore, int healDone, DateTimeOffset? updated, 
        int specializationId, int combatPlayerId)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damageScore, nameof(damageScore));
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healScore, nameof(healScore));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specializationId, nameof(specializationId));

        DamageScore = damageScore;
        DamageDone = damageDone;
        HealScore = healScore;
        HealDone = healDone;
        Updated = updated;
        SpecializationId = specializationId;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; }

    public double DamageScore { get; }

    public int DamageDone { get; }

    public double HealScore { get; }

    public int HealDone { get; }

    public DateTimeOffset? Updated { get; }

    public int SpecializationId { get; }

    public int CombatPlayerId { get; }
}
