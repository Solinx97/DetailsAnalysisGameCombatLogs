using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParserAPI.Models;
namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    CombatLogModel CreateCombatLog(List<string> dungeonNames);

    Task SaveCombatPlayerAsync(CombatModel combat);

    //Task DeleteCombatPlayerDataAsync(CombatPlayerDto combatPlayer);
}
