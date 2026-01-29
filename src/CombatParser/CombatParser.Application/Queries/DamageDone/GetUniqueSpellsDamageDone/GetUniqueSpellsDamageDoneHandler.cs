using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueSpellsDamageDone;

internal class GetUniqueSpellsDamageDoneHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueSpellsDamageDoneQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueSpellsDamageDoneQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
