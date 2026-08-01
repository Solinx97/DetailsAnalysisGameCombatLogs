using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;
using System;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class ResourceRecoveryModel : IDetailsEntity
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int CombatPlayerId { get; set; }
}
