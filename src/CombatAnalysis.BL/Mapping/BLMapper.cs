using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Mapping
{
    public class BLMapper : Profile
    {
        public BLMapper()
        {
            CreateMap<CombatDto, Combat>().ReverseMap();
            CreateMap<CombatPlayerDataDto, CombatPlayerData>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
            CreateMap<HealDoneDto, HealDone>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
            CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        }
    }
}