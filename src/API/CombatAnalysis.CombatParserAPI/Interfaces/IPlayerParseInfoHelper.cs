using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface IPlayerParseInfoHelper
{
    Task UploadPlayerParseInfoAsync(Combat combat, CombatPlayerModel combatPlayer, List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList);
}
