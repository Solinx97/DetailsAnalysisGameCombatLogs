using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.EntityData;

public record CombatPlayerData(
    double AverageItemLevel,
    int ResourcesRecovery,
    int DamageDone,
    int HealDone, 
    int DamageTaken,
    string PlayerId,
    int CombatId,
    IPlayerStats Stats,
    SpecializationScoreData Score,
    IReadOnlyList<CombatPlayerPreAuraData> PreAuras,
    IReadOnlyList<CombatPlayerAuraData> Auras,
    IReadOnlyList<UnitCastData> Casts,
    IReadOnlyList<DamageDoneData> DamageDones,
    IReadOnlyList<DamageDoneGeneralData> DamageDoneGenerals,
    IReadOnlyList<HealDoneData> HealDones,
    IReadOnlyList<HealDoneGeneralData> HealDoneGenerals,
    IReadOnlyList<DamageTakenData> DamageTakens,
    IReadOnlyList<DamageTakenGeneralData> DamageTakenGenerals,
    IReadOnlyList<ResourceRecoveryData> ResourceRecoveries,
    IReadOnlyList<ResourceRecoveryGeneralData> ResourceRecoveryGenerals,
    IReadOnlyList<CombatPlayerDeathData> CombatPlayerDeaths,
    IReadOnlyList<UnitPositionData> CombatPlayerPositions
    );