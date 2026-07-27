using AutoMapper;
using CombatAnalysis.CombatParserAPI.Models;
using CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;
using CombatParser.Application.DTOs;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.EntityData;

namespace CombatAnalysis.CombatParserAPI.Mapping;

internal class CombatParserApiMapper : Profile
{
    public CombatParserApiMapper()
    {
        CreateMap<CombatModel, CombatDto>()
            .ForPath(dest => dest.Boss.Id,
                opt => opt.MapFrom(src => src.Boss.Id));
        CreateMap<CombatDto, CombatModel>()
            .ForPath(dest => dest.Boss.Id,
                opt => opt.MapFrom(src => src.Boss.Id));
        CreateMap<CombatPlayerDto, CombatPlayerModel>()
            .ForPath(dest => dest.Player.Id,
                opt => opt.MapFrom(src => src.PlayerId));
        CreateMap<CombatPlayerModel, CombatPlayerDto>()
            .ForMember(dest => dest.PlayerId,
               opt => opt.MapFrom(src => src.Player.Id));

        CreateMap<PlayerModel, PlayerDto>().ReverseMap();
        CreateMap<BossModel, BossDto>().ReverseMap();
        CreateMap<CombatAbilityModel, CombatAbilityDto>().ReverseMap();
        CreateMap<CombatLogDto, CombatLogModel>().ReverseMap();
        CreateMap<CombatPlayerPreAuraDto, CombatPlayerPreAuraModel>().ReverseMap();
        CreateMap<CombatPlayerAuraDto, CombatPlayerAuraModel>().ReverseMap();
        CreateMap<CombatPlayerCastDto, CombatPlayerCastModel>().ReverseMap();
        CreateMap<UnitPositionDto, UnitPositionModel>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneralModel>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTakenModel>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneralModel>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneralModel>().ReverseMap();
        CreateMap<CombatPlayerDeathDto, CombatPlayerDeathModel>().ReverseMap();
        CreateMap<CombatPlayerStatsDto, CombatPlayerStatsModel>().ReverseMap();
        CreateMap<SpecializationScoreDto, SpecializationScoreModel>().ReverseMap();
        CreateMap<BestSpecializationScoreDto, BestSpecializationScoreModel>().ReverseMap();

        CreateMap<DamageDoneData, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneralData, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDoneData, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneralData, HealDoneGeneralModel>().ReverseMap();
        CreateMap<DamageTakenData, DamageTakenModel>().ReverseMap();
        CreateMap<DamageTakenGeneralData, DamageTakenGeneralModel>().ReverseMap();
        CreateMap<ResourceRecoveryData, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralData, ResourceRecoveryGeneralModel>().ReverseMap();
        CreateMap<CombatUnitData, CombatUnitModel>().ReverseMap();
        CreateMap<UnitHealthData, UnitHealthModel>().ReverseMap();
        CreateMap<UnitPositionData, UnitPositionModel>().ReverseMap();
        CreateMap<CombatPlayerStatsData, CombatPlayerStatsModel>().ReverseMap();
        CreateMap<CombatPlayerDeathData, CombatPlayerDeathModel>().ReverseMap();
        CreateMap<SpecializationScoreData, SpecializationScoreModel>().ReverseMap();
        CreateMap<CombatPlayerPreAuraData, CombatPlayerPreAuraModel>().ReverseMap();
        CreateMap<CombatPlayerAuraData, CombatPlayerAuraModel>().ReverseMap();
        CreateMap<CombatPlayerCastData, CombatPlayerCastModel>().ReverseMap();
    }
}
