using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetWhenPlayerDeath;

public record GetWhenPlayerDeathQuery(
    string UnitId,
    int SkipCount
    ) : IRequest<TimeSpan?>;
