using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenCreators;

internal class GetUniqueDamageTakenCreatorsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetUniqueDamageTakenCreatorsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenCreatorsQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetCreatorNamesAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
