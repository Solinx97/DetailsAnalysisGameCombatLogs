using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageDoneBySpell;

public record CountDamageDoneBySpellQuery(
    int CombatPlayerId,
    string Spell
    ) : IRequest<int>;
