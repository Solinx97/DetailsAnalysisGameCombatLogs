using CombatParser.Domain.EntityData;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombat;

public record CreateCombatCommand(
    string DungeonName,
    double BossHealthPercentage,
    long DamageDone,
    long HealDone,
    long DamageTaken,
    long ResourcesRecovery,
    bool IsWin,
    DateTimeOffset StartDate,
    DateTimeOffset FinishDate,
    int BossId,
    int CombatLogId,
    IReadOnlyList<CombatPlayerData> CombatPlayers,
    IReadOnlyList<CombatAuraData> CombatAuras
    ) : IRequest;
