using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;
using CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;
using System.Collections.Generic;

namespace CombatAnalysis.UploadingLogsApp.Models;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public IPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; }

    public string PlayerId { get; set; }

    public int CombatId { get; set; }

    public double DamageDonePerSecond { get; set; }

    public double HealDonePerSecond { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public double ResourcesRecoveryPerSecond { get; set; }

    public double DamageDonePercentages { get; set; }

    public double HealDonePercentages { get; set; }

    public double DamageTakenPercentages { get; set; }

    public double ResourcesRecoveryPercentages { get; set; }

    public List<CombatPlayerPreAuraModel> PreAuras { get; set; } = [];

    public List<CombatPlayerAuraModel> Auras { get; set; } = [];

    public List<DamageDoneModel> DamageDones { get; set; } = [];

    public List<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    public List<HealDoneModel> HealDones { get; set; } = [];

    public List<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    public List<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    public List<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];

    public List<CombatPlayerDeathModel> CombatPlayerDeathes { get; set; } = [];

    public void ReleaseParsedData()
    {
        PreAuras.Clear();
        Auras.Clear();
        DamageDones.Clear();
        DamageDoneGenerals.Clear();
        HealDones.Clear();
        HealDoneGenerals.Clear();
        ResourceRecoveries.Clear();
        ResourceRecoveryGenerals.Clear();
        CombatPlayerDeathes.Clear();
    }
}
