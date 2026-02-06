using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealCountByCombatPlayerId;

internal class GetHealCountHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetHealCountQuery, int>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(GetHealCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, cancellationToken);

        return count;
    }
}
