using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageValueByTarget;

internal class GetDamageValueByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetDamageValueByTargetQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(GetDamageValueByTargetQuery request, CancellationToken cancellationToken)
    {
        var value = await _repository.GetValueToTargetByCombatPlayerIdAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return value;
    }
}

