using CombatAnalysis.CombatParserAPI.Models;
using CombatAnalysis.CombatParserAPI.Models.Base;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISpecializationScoreHelper
{
    Task CreateSpecializationScoreAsync(CombatPlayerBaseModel combatPlayer, UnitInfoModel unitInfo, int[] spellIds, CancellationToken cancellationToken);
}
