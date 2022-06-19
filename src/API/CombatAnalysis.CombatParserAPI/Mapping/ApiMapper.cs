using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Mapping
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {
            CreateMap<CombatDto, CombatModel>().ReverseMap();
            CreateMap<CombatPlayerDataDto, CombatPlayerDataModel>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
            CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTakenModel>().ReverseMap();
        }
    }
}
