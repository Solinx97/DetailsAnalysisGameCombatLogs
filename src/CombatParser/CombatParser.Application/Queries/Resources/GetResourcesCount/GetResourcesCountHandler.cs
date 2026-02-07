using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesCount;

internal class GetResourcesCountHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetResourcesCountQuery, int>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(GetResourcesCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, cancellationToken);

        return count;
    }
}
