namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoCountService<TModel> : IPlayerInfoService<TModel>
    where TModel : class
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}
