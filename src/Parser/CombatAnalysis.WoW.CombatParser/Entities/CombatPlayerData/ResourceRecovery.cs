using CombatAnalysis.WoW.CombatParser.Entities.Base;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class ResourceRecovery : CombatUnitDataBase, ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
