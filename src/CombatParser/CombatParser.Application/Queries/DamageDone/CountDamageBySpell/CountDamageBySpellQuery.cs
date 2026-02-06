using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageBySpell;

public record CountDamageBySpellQuery(
    int CombatPlayerId,
    string Spell
    ) : IRequest<int>;
