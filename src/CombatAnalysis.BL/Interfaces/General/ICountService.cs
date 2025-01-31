namespace CombatAnalysis.BL.Interfaces.General;

public interface ICountService<TModel>
    where TModel : class
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}