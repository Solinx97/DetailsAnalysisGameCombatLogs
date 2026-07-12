using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesCreators;

internal class GetUniqueResourcesCreatorHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<GetUniqueResourcesCreatorsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueResourcesCreatorsQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetUniqueTargetsAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
