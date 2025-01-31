using CombatAnalysis.CombatParserAPI.Models;
namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    Task SaveCombatPlayerAsync(CombatModel combat);
}
