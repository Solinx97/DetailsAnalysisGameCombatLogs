using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageByTarget;

internal class CountDamageByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageByTargetQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageByTargetQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByTargetAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return count;
    }
}
