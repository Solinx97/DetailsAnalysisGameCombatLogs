using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetDamageTakens;

public record GetDamageTakensQuery(
    int CombatPlayerId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<DamageDoneDto>>;
