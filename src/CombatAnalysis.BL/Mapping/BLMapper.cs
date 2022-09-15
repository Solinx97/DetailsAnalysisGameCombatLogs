using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.User;

namespace CombatAnalysis.BL.Mapping
{
    public class BLMapper : Profile
    {
        public BLMapper()
        {
            CreateMap<AppUserDto, AppUser>().ReverseMap();
            CreateMap<PersonalChatDto, PersonalChat>().ReverseMap();
            CreateMap<PersonalChatMessageDto, PersonalChatMessage>().ReverseMap();
            CreateMap<CombatLogDto, CombatLog>().ReverseMap();
            CreateMap<CombatLogByUserDto, CombatLogByUser>().ReverseMap();
            CreateMap<CombatDto, Combat>().ReverseMap();
            CreateMap<CombatPlayerDto, CombatPlayer>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
            CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
            CreateMap<HealDoneDto, HealDone>().ReverseMap();
            CreateMap<HealDoneGeneralDto, HealDoneGeneral>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
            CreateMap<DamageTakenGeneralDto, DamageTakenGeneral>().ReverseMap();
            CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
            CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>().ReverseMap();
        }
    }
}