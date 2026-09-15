using System;

namespace CombatAnalysis.UploadingLogsApp.Models;

public class UnitCastModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan Time { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string? TargetGameId { get; set; }

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    public string UnitId { get; set; } = string.Empty;
}
