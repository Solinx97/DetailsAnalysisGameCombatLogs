namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoFilterService<TModel>
    where TModel : class
{
    Task<IEnumerable<object>> GetUniqueParamByCombatPlayerIdAsync(int combatPlayerId, string paramName);

    Task<IEnumerable<TModel>> GetParamByCombatPlayerIdAsync(int combatPlayerId, string paramName, object value);
}
