using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitCasts;

public record GetUnitCastsQuery(
    string CombatUnitId
    ) : IRequest<IDictionary<string, IEnumerable<UnitCastDto>>>;
