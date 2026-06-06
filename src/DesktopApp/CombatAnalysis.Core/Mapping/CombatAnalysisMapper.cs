using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.Mapping;

internal class CombatAnalysisMapper : Profile
{
    public CombatAnalysisMapper()
    {
        CreateMap<PlayerModel, Player>().ReverseMap();
        CreateMap<BossModel, Boss>().ReverseMap();
        CreateMap<CombatModel, Combat>().ReverseMap();
        CreateMap<CombatPlayerModel, CombatPlayer>().ReverseMap();
        CreateMap<CombatPlayerStatsModel, PlayerStats>().ReverseMap();
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