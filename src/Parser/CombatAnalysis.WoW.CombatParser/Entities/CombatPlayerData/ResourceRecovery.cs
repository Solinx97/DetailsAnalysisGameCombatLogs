using CombatAnalysis.WoW.CombatParser.Entities.Base;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class ResourceRecovery : CombatUnitDataBase, ICombatPlayerEntity, ICombatPlayerResourceRefs
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public int ModificationType { get; set; }

    public int CombatPlayerId { get; set; }
}
