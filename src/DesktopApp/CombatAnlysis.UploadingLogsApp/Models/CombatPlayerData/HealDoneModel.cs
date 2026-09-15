using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;
using System;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class HealDoneModel : IDetailsEntity
{
    public string Id { get; set; } = string.Empty;

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public string TargetGameId { get; set; }

    public int ModificationType { get; set; }

    public string UnitId { get; set; } = string.Empty;
}
