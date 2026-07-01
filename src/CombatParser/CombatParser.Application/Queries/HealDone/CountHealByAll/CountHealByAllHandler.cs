using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealByAll;

internal class CountHealByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<CountHealByAllQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(CountHealByAllQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByAllTargetsAsync(request.CombatPlayerId, request.Target, request.Spell, cancellationToken);

        return count;
    }
}