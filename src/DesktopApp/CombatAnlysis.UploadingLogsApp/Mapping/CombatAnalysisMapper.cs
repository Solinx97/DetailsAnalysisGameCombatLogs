using AutoMapper;
using CombatAnalysis.UploadingLogsApp.Entities.WoWMidnight;
using CombatAnalysis.UploadingLogsApp.Entities.WoWMoPClassic;
using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;
using CombatAnalysis.UploadingLogsApp.Models;
using CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.UploadingLogsApp.Mapping;

internal class CombatAnalysisMapper : Profile
{
    public CombatAnalysisMapper()
    {
        CreateMap<PlayerModel, Player>().ReverseMap();
        CreateMap<BossModel, Boss>().ReverseMap();
        CreateMap<CombatModel, Combat>().ReverseMap();
        CreateMap<CombatUnitModel, CombatUnit>().ReverseMap();
        CreateMap<UnitPositionModel, UnitPosition>().ReverseMap();
        CreateMap<CombatPlayerModel, CombatPlayer>().ReverseMap();
        CreateMap<UnitCastModel, UnitCast>().ReverseMap();

        CreateMap<IPlayerStatsModel, IPlayerStats>()
            .Include<WoWMoPClassicPlayerStatsModel, WoW_5_5_4.CombatParser.Entities.PlayerStats>()
            .Include<WoWMidnightPlayerStatsModel, WoW_12_1_0.CombatParser.Entities.PlayerStats>().ReverseMap();

        CreateMap<WoWMoPClassicPlayerStatsModel, WoW_5_5_4.CombatParser.Entities.PlayerStats>().ReverseMap();
        CreateMap<WoWMidnightPlayerStatsModel, WoW_12_1_0.CombatParser.Entities.PlayerStats>().ReverseMap();

        CreateMap<SpecializationScoreModel, SpecializationScore>().ReverseMap();
        CreateMap<DamageDoneModel, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralModel, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneModel, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralModel, HealDoneGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryModel, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralModel, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<CombatPlayerDeathModel, CombatPlayerDeath>().ReverseMap();
        CreateMap<CombatPlayerPreAuraModel, CombatPlayerPreAura>().ReverseMap();
        CreateMap<CombatPlayerAuraModel, CombatPlayerAura>().ReverseMap();
    }
}