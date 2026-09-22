using CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;
using System.Collections.Generic;

namespace CombatAnalysis.UploadingLogsApp.Models;

public class UnitModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }

    public UnitInfoModel UnitInfo { get; set; } = new();

    public List<UnitHealthModel> UnitHealthes { get; set; } = [];

    public List<UnitCastModel> UnitCasts { get; set; } = [];

    public List<UnitPositionModel> UnitPositions { get; set; } = [];

    public List<UnitPreAuraModel> PreAuras { get; set; } = [];

    public List<UnitAuraModel> Auras { get; set; } = [];

    public List<DamageDoneModel> DamageDones { get; set; } = [];

    public List<DamageDoneModel> DamageTakens { get; set; } = [];

    public List<HealDoneModel> HealDones { get; set; } = [];

    public List<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    public void ReleaseParsedData()
    {
        UnitHealthes.Clear();
        UnitCasts.Clear();
        UnitPositions.Clear();
        PreAuras.Clear();
        Auras.Clear();
        DamageDones.Clear();
        DamageTakens.Clear();
        HealDones.Clear();
        ResourceRecoveries.Clear();
    }
}
