namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoService<TModel>
    where TModel : class
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId);

    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize);
}