namespace CombatParser.Domain.EntityData;

public record SpecializationScoreData(
    double DamageScore,
    int DamageDone,
    double HealScore, 
    int HealDone,
    DateTimeOffset? Updated,
    int SpecializationId,
    int CombatPlayerId
    );
