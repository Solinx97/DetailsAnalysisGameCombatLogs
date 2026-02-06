using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealByTarget;

internal class CountHealByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<CountHealByTargetQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(CountHealByTargetQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByTargetAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return count;
    }
}
