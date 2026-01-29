using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageDoneByTarget;

internal class CountDamageDoneByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageDoneByTargetQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageDoneByTargetQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountDamageDoneByTargetAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return count;
    }
}
