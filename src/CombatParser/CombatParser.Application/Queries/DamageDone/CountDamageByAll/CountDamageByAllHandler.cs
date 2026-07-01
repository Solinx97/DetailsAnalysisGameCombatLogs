using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageByAll;

internal class CountDamageByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageByAllQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageByAllQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountByAllTargetsAsync(request.CombatPlayerId, request.Target, request.Spell, cancellationToken);

        return count;
    }
}