namespace CombatParser.Domain.Entities.CombatPlayerData;

public class SpecializationScore : CombatPlayerDataBase
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

    public double DamageScore { get; private set; }

    public int DamageDone { get; private set; }

    public double HealScore { get; private set; }

    public int HealDone { get; private set; }

    public DateTimeOffset? Updated { get; private set; }

    public Specialization Specialization { get; private set; }

    public int SpecializationId { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public void SetScore(int bestSpecialziationDamageDone, int bestSpecialziationHealDone)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(bestSpecialziationDamageDone, nameof(bestSpecialziationDamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(bestSpecialziationHealDone, nameof(bestSpecialziationHealDone));

        if (bestSpecialziationDamageDone == 0 || DamageDone >= bestSpecialziationDamageDone)
        {
            DamageScore = 100;
        }
        else
        {
            DamageScore = (double)DamageDone / (double)bestSpecialziationDamageDone;
        }

        if (bestSpecialziationHealDone == 0 || HealDone >= bestSpecialziationHealDone)
        {
            HealScore = 100;
        }
        else
        {
            HealScore = (double)HealDone / (double)bestSpecialziationHealDone;
        }

        Updated = DateTimeOffset.UtcNow;
    }

    public void Update(double damageScore, double healScore)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damageScore, nameof(damageScore));
        ArgumentOutOfRangeException.ThrowIfNegative(healScore, nameof(healScore));

        if (DamageScore != damageScore)
        {
            DamageScore = damageScore;
            Updated = DateTimeOffset.UtcNow;
        }

        if (HealScore != healScore)
        {
            HealScore = healScore;
            Updated = DateTimeOffset.UtcNow;
        }
    }
}
