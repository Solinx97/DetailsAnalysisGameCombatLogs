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
            CreateMap<DamageDoneModel, DamageDone>().ReverseMap();
            CreateMap<HealDoneModel, HealDone>().ReverseMap();
            CreateMap<DamageTakenModel, DamageTaken>().ReverseMap();
        }
    }
}