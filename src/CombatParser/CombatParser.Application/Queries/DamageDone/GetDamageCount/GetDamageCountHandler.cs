using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageCount;

internal class GetDamageCountHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetDamageCountQuery, int>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(GetDamageCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, cancellationToken);

        return count;
    }
}
