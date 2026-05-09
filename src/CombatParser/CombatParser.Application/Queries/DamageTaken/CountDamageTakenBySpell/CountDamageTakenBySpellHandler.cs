using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenBySpell;

internal class CountDamageTakenBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<CountDamageTakenBySpellQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<int> Handle(CountDamageTakenBySpellQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountBySpellAsync(request.CombatPlayerId, request.Spell, cancellationToken);

        return count;
    }
}

