using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Models;

namespace CombatAnalysis.Core.Mapping;

public class CombatAnalysisMapper : Profile
{
    public CombatAnalysisMapper()
    {
        CreateMap<CombatModel, Combat>().ReverseMap();
        CreateMap<CombatPlayerModel, CombatPlayer>().ReverseMap();
        CreateMap<PlayerStatsModel, PlayerStats>().ReverseMap();
        CreateMap<DamageDoneModel, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralModel, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneModel, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralModel, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenModel, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralModel, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryModel, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralModel, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<PlayerDeathModel, PlayerDeath>().ReverseMap();
    }
}