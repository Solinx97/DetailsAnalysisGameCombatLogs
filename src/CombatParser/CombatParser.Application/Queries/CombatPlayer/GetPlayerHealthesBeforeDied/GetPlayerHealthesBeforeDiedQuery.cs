using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerHealthesBeforeDied;

public record GetPlayerHealthesBeforeDiedQuery(
    string UnitId,
    string To
    ) : IRequest<IEnumerable<UnitHealthDto>>;
