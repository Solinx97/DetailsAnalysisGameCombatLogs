namespace CombatParser.Domain.EntityData;

public record UnitData(
    string GameId,
    string Name,
    string UnitHash,
    int Type,
    string? CreatorGameId,
    int CombatId,
    UnitInfoData UnitInfo,
    IReadOnlyList<UnitHealthData> UnitHealths,
    IReadOnlyList<UnitCastData> UnitCasts,
    IReadOnlyList<UnitPositionData> UnitPositions,
    IReadOnlyList<UnitPreAuraData> PreAuras,
    IReadOnlyList<UnitAuraData> Auras,
    IReadOnlyList<DamageDoneData> DamageDones,
    IReadOnlyList<DamageDoneGeneralData> DamageDoneGenerals,
    IReadOnlyList<HealDoneData> HealDones,
    IReadOnlyList<HealDoneGeneralData> HealDoneGenerals,
    IReadOnlyList<ResourceRecoveryData> ResourceRecoveries,
    IReadOnlyList<ResourceRecoveryGeneralData> ResourceRecoveryGenerals
    );