using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitCasts;

public record GetUnitCastsQuery(
    string CombatUnitId
    ) : IRequest<IEnumerable<UnitCastDto>>;
