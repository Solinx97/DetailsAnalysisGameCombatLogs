using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenBySpell;

public record CountDamageTakenBySpellQuery(
    int CombatPlayerId,
    string Spell
    ) : IRequest<int>;
