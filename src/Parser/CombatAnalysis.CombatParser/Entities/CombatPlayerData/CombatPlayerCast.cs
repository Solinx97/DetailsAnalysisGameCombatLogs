using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerCast : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    public int CombatPlayerId { get; set; }
}
