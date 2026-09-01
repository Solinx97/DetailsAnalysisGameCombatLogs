using AutoMapper;
using CombatAnalysis.WoW_5_5_4.CombatParser.Entities;
using CombatAnalysis.WoW_5_5_4.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.UploadingLogsApp.Models;
using CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

namespace CombatAnalysis.UploadingLogsApp.Mapping;

internal class CombatAnalysisMapper : Profile
{
    public CombatAnalysisMapper()
    {
        CreateMap<PlayerModel, Player>().ReverseMap();
        CreateMap<BossModel, Boss>().ReverseMap();
        CreateMap<CombatModel, Combat>().ReverseMap();
        CreateMap<CombatUnitModel, CombatUnit>().ReverseMap();
        CreateMap<UnitHealthModel, UnitHealth>().ReverseMap();
        CreateMap<UnitPositionModel, UnitPosition>().ReverseMap();
        CreateMap<CombatPlayerModel, CombatPlayer>().ReverseMap();
        CreateMap<UnitCastModel, UnitCast>().ReverseMap();
        CreateMap<CombatPlayerStatsModel, CombatPlayerStats>().ReverseMap();
        CreateMap<SpecializationScoreModel, SpecializationScore>().ReverseMap();
        CreateMap<DamageDoneModel, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralModel, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneModel, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralModel, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenModel, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralModel, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryModel, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralModel, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<CombatPlayerDeathModel, CombatPlayerDeath>().ReverseMap();
        CreateMap<CombatPlayerPreAuraModel, CombatPlayerPreAura>().ReverseMap();
        CreateMap<CombatPlayerAuraModel, CombatPlayerAura>().ReverseMap();
    }
}