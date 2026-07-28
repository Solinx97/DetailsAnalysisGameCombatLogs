using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitCasts;

public record GetUnitCastsQuery(
    int CombatId
    ) : IRequest<IDictionary<string, IEnumerable<UnitCastDto>>>;
