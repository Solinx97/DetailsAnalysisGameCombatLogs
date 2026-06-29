using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageByAll;

public record CountDamageByAllQuery(
    int CombatPlayerId,
    string Target,
    string Spell
    ) : IRequest<int>;
