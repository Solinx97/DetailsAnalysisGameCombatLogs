using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageDoneBySpell;

internal class CountDamageDoneBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageDoneBySpellQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageDoneBySpellQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountDamageDoneBySpellAsync(request.CombatPlayerId, request.Spell, cancellationToken);

        return count;
    }
}

