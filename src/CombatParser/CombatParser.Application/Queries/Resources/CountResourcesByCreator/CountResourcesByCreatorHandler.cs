using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourcesByCreator;

internal class CountResourcesByCreatorHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<CountResourcesByCreatorQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<int> Handle(CountResourcesByCreatorQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByTargetAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return count;
    }
}
