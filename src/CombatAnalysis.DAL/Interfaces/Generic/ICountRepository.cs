namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface ICountRepository
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}