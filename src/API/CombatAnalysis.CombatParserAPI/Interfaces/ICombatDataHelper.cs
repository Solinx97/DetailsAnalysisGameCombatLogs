using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParserAPI.Models;
namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    CombatLogModel CreateCombatLog(List<string> dungeonNames);

    Task SaveCombatPlayerDataAsync(CombatDto combat, Dictionary<string, List<string>> petsId, CombatPlayerDto combatPlayer, List<string> combatData);

    Task DeleteCombatPlayerDataAsync(CombatPlayerDto combatPlayer);
}
