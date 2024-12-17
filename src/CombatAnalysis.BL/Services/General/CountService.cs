using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services.General;

internal class CountService<TModel, TModelMap> : ICountService<TModel>
    where TModel : class
    where TModelMap : class, IEntity
{
    private readonly ICountRepository<TModelMap> _countRepository;

    public CountService(ICountRepository<TModelMap> countRepository)
    {
        _countRepository = countRepository;
    }

    public async Task<int> CountByCombatPlayerIdAsync(int combatPlayerId)
    {
        var count = await _countRepository.CountByCombatPlayerIdAsync(combatPlayerId);

        return count;
    }
}
