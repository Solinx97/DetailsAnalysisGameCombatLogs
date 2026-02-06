using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageBySpell;

internal class CountDamageBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageBySpellQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageBySpellQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountBySpellAsync(request.CombatPlayerId, request.Spell, cancellationToken);

        return count;
    }
}

