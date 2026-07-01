using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourceByAll;

internal class CountResourceByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<CountResourceByAllQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<int> Handle(CountResourceByAllQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByAllCreatorsAsync(request.CombatPlayerId, request.Creator, request.Spell, cancellationToken);

        return count;
    }
}
