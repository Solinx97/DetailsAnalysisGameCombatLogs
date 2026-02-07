using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenByCreator;

internal class CountDamageTakenByCreatorHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<CountDamageTakenByCreatorQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<int> Handle(CountDamageTakenByCreatorQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByCreatorAsync(request.CombatPlayerId, request.Target, cancellationToken);

        return count;
    }
}
