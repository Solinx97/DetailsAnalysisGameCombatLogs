using CombatAnalysis.CombatParserAPI.Models;
namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISaveCombatDataHelper
{
    CombatLogModel CreateCombatLog(List<string> dungeonNames);

    Task SaveCombatPlayerDataAsync(int combatId, CombatPlayerModel combatPlayer);
}
