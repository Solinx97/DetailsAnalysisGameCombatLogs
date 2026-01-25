namespace CombatParser.Domain.Entities;

public class SpecializationScore
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

    public int Id { get; private set; }

    public double DamageScore { get; private set; }

    public int DamageDone { get; private set; }

    public double HealScore { get; private set; }

    public int HealDone { get; private set; }

    public DateTimeOffset? Updated { get; private set; }

    public Specialization Specialization { get; private set; }

    public int SpecializationId { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public int CombatPlayerId { get; private set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}
