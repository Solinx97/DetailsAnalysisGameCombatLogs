using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensByCreator;

public record GetDamageTakensByCreatorQuery(
    int CombatPlayerId,
    string Creator,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageTakenDto>>;