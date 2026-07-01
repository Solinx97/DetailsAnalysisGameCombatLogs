using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensByAll;

public record GetDamageTakensByAllQuery(
    int CombatPlayerId,
    string Creator,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageTakenDto>>;
