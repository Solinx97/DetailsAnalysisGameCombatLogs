using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourcesBySpell;

internal class CountResourcesBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<CountResourcesBySpellQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<int> Handle(CountResourcesBySpellQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountBySpellAsync(request.CombatPlayerId, request.Spell, cancellationToken);

        return count;
    }
}

