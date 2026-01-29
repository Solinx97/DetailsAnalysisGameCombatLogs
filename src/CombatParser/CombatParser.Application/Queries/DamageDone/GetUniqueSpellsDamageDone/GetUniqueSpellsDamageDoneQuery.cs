using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueSpellsDamageDone;

public record GetUniqueSpellsDamageDoneQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
