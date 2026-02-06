using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageValueToTarget;

internal class GetDamageValueToTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetDamageValueToTargetQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(GetDamageValueToTargetQuery request, CancellationToken cancellationToken)
    {
        var value = await _repository.GetValueToTargetAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return value;
    }
}

