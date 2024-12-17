using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecScore
{
    Task<IEnumerable<SpecializationScore>> GetBySpecIdAsync(int specId, int bossId, int difficult);
}