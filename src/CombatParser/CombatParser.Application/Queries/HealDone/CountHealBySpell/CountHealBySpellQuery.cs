using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealBySpell;

public record CountHealBySpellQuery(
    int CombatPlayerId,
    string Spell
    ) : IRequest<int>;
