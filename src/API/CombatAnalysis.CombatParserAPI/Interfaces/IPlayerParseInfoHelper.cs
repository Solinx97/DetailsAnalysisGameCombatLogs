using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface IPlayerParseInfoHelper
{
    Task UploadPlayerParseInfoAsync(CombatDto combat, CombatPlayerDto combatPlayer, List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList);
}
