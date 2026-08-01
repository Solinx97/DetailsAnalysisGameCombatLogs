using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class HealDoneGeneralModel : IGeneralDetailsEntity
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public double HealPerSecond { get; set; }

    public int CritNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
