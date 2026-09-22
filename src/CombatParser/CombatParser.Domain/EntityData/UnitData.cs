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
    IReadOnlyList<HealDoneData> HealDones,
    IReadOnlyList<ResourceRecoveryData> ResourceRecoveries
    );