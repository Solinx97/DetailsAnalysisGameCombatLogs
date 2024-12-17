using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services.General;

internal class CountService<TModel> : ICountService<TModel>
    where TModel : class
{
    private readonly ICountRepository _countRepository;

    public CountService(ICountRepository countRepository)
    {
        _countRepository = countRepository;
    }

    public async Task<int> CountByCombatPlayerIdAsync(int combatPlayerId)
    {
        var count = await _countRepository.CountByCombatPlayerIdAsync(combatPlayerId);

        return count;
    }
}
