using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParserAPI.Models;
namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISaveCombatDataHelper
{
    CombatLogModel CreateCombatLog(List<string> dungeonNames);

    Task SaveCombatPlayerDataAsync(CombatDto combat, CombatPlayerDto combatPlayer, List<string> combatData);
}
