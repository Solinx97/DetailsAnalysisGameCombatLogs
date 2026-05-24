namespace CombatParser.Domain.EntityData;

public record CombatPlayerData(
    double AverageItemLevel,
    int ResourcesRecovery,
    int DamageDone,
    int HealDone, 
    int DamageTaken,
    string PlayerId,
    int CombatId,
    CombatPlayerStatsData Stats,
    SpecializationScoreData Score,
    IReadOnlyList<CombatPlayerAuraData> Auras,
    IReadOnlyList<DamageDoneData> DamageDones,
    IReadOnlyList<DamageDoneGeneralData> DamageDoneGenerals,
    IReadOnlyList<HealDoneData> HealDones,
    IReadOnlyList<HealDoneGeneralData> HealDoneGenerals,
    IReadOnlyList<DamageTakenData> DamageTakens,
    IReadOnlyList<DamageTakenGeneralData> DamageTakenGenerals,
    IReadOnlyList<ResourceRecoveryData> ResourceRecoveries,
    IReadOnlyList<ResourceRecoveryGeneralData> ResourceRecoveryGenerals,
    IReadOnlyList<CombatPlayerDeathData> CombatPlayerDeaths,
    IReadOnlyList<CombatPlayerPositionData> CombatPlayerPositions
    );