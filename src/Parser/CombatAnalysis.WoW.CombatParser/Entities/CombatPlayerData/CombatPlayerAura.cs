using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerAura : ICombatPlayerEntity
{
    public int GameAuraId { get; set; }

    public string Name { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int AuraCreatorType { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }

    public int CombatPlayerId { get; set; }
}
