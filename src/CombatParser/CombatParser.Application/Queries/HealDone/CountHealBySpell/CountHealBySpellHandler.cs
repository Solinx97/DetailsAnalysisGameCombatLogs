using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealBySpell;

internal class CountHealBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<CountHealBySpellQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(CountHealBySpellQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountBySpellAsync(request.CombatPlayerId, request.Spell, cancellationToken);

        return count;
    }
}

