using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenByAll;

internal class CountDamageTakenByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<CountDamageTakenByAllQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<int> Handle(CountDamageTakenByAllQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByAllCreatorsAsync(request.CombatPlayerId, request.Creator, request.Spell, cancellationToken);

        return count;
    }
}