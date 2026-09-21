using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUniqueUnitNames;

public record GetUniqueUnitNamesQuery(
    int CombatLogId,
    string BossName
    ) : IRequest<IEnumerable<UniqueUnitNameDto>>;
