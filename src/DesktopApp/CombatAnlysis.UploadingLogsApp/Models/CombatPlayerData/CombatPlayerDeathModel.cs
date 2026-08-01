using System;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class CombatPlayerDeathModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
