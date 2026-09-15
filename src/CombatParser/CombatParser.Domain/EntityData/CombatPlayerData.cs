using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.EntityData;

public record CombatPlayerData(
    double AverageItemLevel,
    string PlayerId,
    IPlayerStatsData Stats,
    SpecializationScoreData Score,
    string UnitGameId
    );