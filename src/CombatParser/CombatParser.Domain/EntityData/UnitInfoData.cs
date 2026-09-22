namespace CombatParser.Domain.EntityData;

public record UnitInfoData(
    int ResourcesRecovery,
    int DamageDone,
    int HealDone,
    int DamageTaken
    );
