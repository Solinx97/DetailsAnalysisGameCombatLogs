using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecScore : IGenericRepository<SpecializationScore>
{
    Task<IEnumerable<SpecializationScore>> GetBySpecIdAsync(int specId, int bossId, int difficult);
}