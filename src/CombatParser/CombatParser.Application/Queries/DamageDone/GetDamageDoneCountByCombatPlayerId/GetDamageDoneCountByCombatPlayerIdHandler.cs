using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDoneCountByCombatPlayerId;

internal class GetDamageDoneCountByCombatPlayerIdHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetDamageDoneCountByCombatPlayerIdQuery, int>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(GetDamageDoneCountByCombatPlayerIdQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, cancellationToken);

        return count;
    }
}
