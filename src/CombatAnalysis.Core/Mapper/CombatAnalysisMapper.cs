using AutoMapper;
using CombatAnalysis.CombatParser.Models;
using CombatAnalysis.Core.Models;

namespace CombatAnalysis.Core.Mapper
{
    public class CombatAnalysisMapper : Profile
    {
        public CombatAnalysisMapper()
        {
            CreateMap<CombatModel, Combat>().ReverseMap();
            CreateMap<PlayerCombatModel, PlayerCombat>().ReverseMap();
            CreateMap<DamageDoneInformationModel, DamageDoneInformation>().ReverseMap();
            CreateMap<HealDoneInformationModel, HealDoneInformation>().ReverseMap();
            CreateMap<DamageTakenInformationModel, DamageTakenInformation>().ReverseMap();
        }
    }
}