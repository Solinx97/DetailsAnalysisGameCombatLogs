using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Mapping;

public class CombatParserApiMapper : Profile
{
    public CombatParserApiMapper()
    {
        CreateMap<CombatLogDto, CombatLogModel>().ReverseMap();
        CreateMap<CombatLogByUserDto, CombatLogByUserModel>().ReverseMap();
        CreateMap<CombatDto, CombatModel>().ReverseMap();
        CreateMap<CombatDto, Combat>().ReverseMap();
        CreateMap<CombatPlayerDto, CombatPlayerModel>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneralModel>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTakenModel>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneralModel>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneralModel>().ReverseMap();
        CreateMap<PlayerDeathDto, PlayerDeathModel>().ReverseMap();

        CreateMap<Combat, CombatModel>().ReverseMap();
        CreateMap<DamageDone, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneral, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDone, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneral, HealDoneGeneralModel>().ReverseMap();
        CreateMap<DamageTaken, DamageTakenModel>().ReverseMap();
        CreateMap<DamageTakenGeneral, DamageTakenGeneralModel>().ReverseMap();
        CreateMap<ResourceRecovery, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneral, ResourceRecoveryGeneralModel>().ReverseMap();

        CreateMap<PlayerParseInfoDto, PlayerParseInfoModel>().ReverseMap();

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
