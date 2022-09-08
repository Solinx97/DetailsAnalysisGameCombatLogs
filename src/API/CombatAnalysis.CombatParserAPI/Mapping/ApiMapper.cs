using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Mapping
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {
            CreateMap<CombatLogDto, CombatLogModel>().ReverseMap();
            CreateMap<CombatLogByUserDto, CombatLogByUserModel>().ReverseMap();
            CreateMap<CombatDto, CombatModel>().ReverseMap();
            CreateMap<CombatModel, Combat>().ReverseMap();
            CreateMap<CombatPlayerDataDto, CombatPlayerDataModel>().ReverseMap();
            CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
            CreateMap<DamageDoneGeneralDto, DamageDoneGeneralModel>().ReverseMap();
            CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
            CreateMap<HealDoneGeneralDto, HealDoneGeneralModel>().ReverseMap();
            CreateMap<DamageTakenDto, DamageTakenModel>().ReverseMap();
            CreateMap<DamageTakenGeneralDto, DamageTakenGeneralModel>().ReverseMap();
            CreateMap<ResourceRecoveryDto, ResourceRecoveryModel>().ReverseMap();
            CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneralModel>().ReverseMap();
            CreateMap<DamageDone, DamageDoneModel>().ReverseMap();
            CreateMap<DamageDoneGeneral, DamageDoneGeneralModel>().ReverseMap();
            CreateMap<HealDone, HealDoneModel>().ReverseMap();
            CreateMap<HealDoneGeneral, HealDoneGeneralModel>().ReverseMap();
            CreateMap<DamageTaken, DamageTakenModel>().ReverseMap();
            CreateMap<DamageTakenGeneral, DamageTakenGeneralModel>().ReverseMap();
            CreateMap<ResourceRecovery, ResourceRecoveryModel>().ReverseMap();
            CreateMap<ResourceRecoveryGeneral, ResourceRecoveryGeneralModel>().ReverseMap();
        }
    }
}
