using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.CountDamageTaken;

internal class CountDamageTakenHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageTakenQuery, int>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageTakenQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.UnitId, request.Target, request.Creator, request.Spell, request.From, request.To, cancellationToken);

        return count;
    }
}