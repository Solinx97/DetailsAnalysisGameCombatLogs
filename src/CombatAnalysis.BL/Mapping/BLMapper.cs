using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Mapping
{
    public class BLMapper : Profile
    {
        public BLMapper()
        {
            CreateMap<CombatLogDto, CombatLog>().ReverseMap();
            CreateMap<CombatDto, Combat>().ReverseMap();
            CreateMap<CombatPlayerDataDto, CombatPlayerData>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
            CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
            CreateMap<HealDoneDto, HealDone>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
            CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        }
    }
}