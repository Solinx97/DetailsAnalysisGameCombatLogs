using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenCreators;

internal class GetUniqueDamageTakenCreatorsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageTakenCreatorsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenCreatorsQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetCreatorNamesAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
