using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourcesBySpell;

public record CountResourcesBySpellQuery(
    int CombatPlayerId,
    string Spell
    ) : IRequest<int>;
