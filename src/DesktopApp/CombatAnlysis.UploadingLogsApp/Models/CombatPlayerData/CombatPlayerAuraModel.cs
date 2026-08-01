using System;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class CombatPlayerAuraModel
{
    public int Id { get; set; }

    public int GameAuraId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public int AuraCreatorType { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }

    public int CombatPlayerId { get; set; }
}
