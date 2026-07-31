using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.Core.Mapping;

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