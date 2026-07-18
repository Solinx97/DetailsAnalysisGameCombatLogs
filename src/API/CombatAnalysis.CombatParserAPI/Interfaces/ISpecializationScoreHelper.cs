using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISpecializationScoreHelper
{
    Task CreateSpecializationScoreAsync(CombatPlayerModel combatPlayer, int[] spellIds, CancellationToken cancellationToken);
}
