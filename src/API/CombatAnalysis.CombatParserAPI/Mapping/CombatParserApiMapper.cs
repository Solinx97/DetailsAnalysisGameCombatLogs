using AutoMapper;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;
using CombatAnalysis.CombatParserAPI.Models.WoWMidnight;
using CombatAnalysis.CombatParserAPI.Models.WoWMoPClassic;
using CombatParser.Application.DTOs;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Application.DTOs.WoWMidnight;
using CombatParser.Application.DTOs.WoWMoPClassic;
using CombatParser.Application.Interfaces;
using CombatParser.Domain.EntityData;
using CombatParser.Domain.EntityData.WoWMidnight;
using CombatParser.Domain.EntityData.WoWMoPClassic;
using CombatParser.Domain.Interfaces;

namespace CombatAnalysis.CombatParserAPI.Mapping;

internal class CombatParserApiMapper : Profile
{
    public CombatParserApiMapper()
    {
        CreateMap<CombatModel, CombatDto>().ReverseMap();
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
        CreateMap<UnitPreAuraDto, UnitPreAuraModel>().ReverseMap();
        CreateMap<UnitAuraDto, UnitAuraModel>().ReverseMap();
        CreateMap<UnitInfoDto, UnitInfoModel>().ReverseMap();
        CreateMap<UnitCastDto, UnitCastModel>().ReverseMap();
        CreateMap<UnitPositionDto, UnitPositionModel>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDoneDto, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneralModel>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneralModel>().ReverseMap();
        CreateMap<SpecializationScoreDto, SpecializationScoreModel>().ReverseMap();
        CreateMap<BestSpecializationScoreDto, BestSpecializationScoreModel>().ReverseMap();

        CreateMap<DamageDoneData, DamageDoneModel>().ReverseMap();
        CreateMap<HealDoneData, HealDoneModel>().ReverseMap();
        CreateMap<ResourceRecoveryData, ResourceRecoveryModel>().ReverseMap();
        CreateMap<UnitData, UnitModel>().ReverseMap();
        CreateMap<UnitInfoData, UnitInfoModel>().ReverseMap();
        CreateMap<UnitHealthData, UnitHealthModel>().ReverseMap();
        CreateMap<UnitPositionData, UnitPositionModel>().ReverseMap();
        CreateMap<UnitCastData, UnitCastModel>().ReverseMap();

        CreateMap<SpecializationScoreData, SpecializationScoreModel>().ReverseMap();
        CreateMap<UnitPreAuraData, UnitPreAuraModel>().ReverseMap();
        CreateMap<UnitAuraData, UnitAuraModel>().ReverseMap();

        CreateMap<IPlayerStatsModel, IPlayerStatsData>()
            .Include<WoWMoPClassicPlayerStatsModel, WoWMoPClassicPlayerStatsData>()
            .Include<WoWMidnightPlayerStatsModel, WoWMidnightPlayerStatsData>().ReverseMap();

        CreateMap<WoWMoPClassicPlayerStatsData, WoWMoPClassicPlayerStatsModel>().ReverseMap();
        CreateMap<WoWMidnightPlayerStatsData, WoWMidnightPlayerStatsModel>().ReverseMap();

        CreateMap<IPlayerStatsModel, IPlayerStatsDto>()
            .Include<WoWMoPClassicPlayerStatsModel, WoWMoPClassicPlayerStatsDto>()
            .Include<WoWMidnightPlayerStatsModel, WoWMidnightPlayerStatsDto>().ReverseMap();

        CreateMap<WoWMoPClassicPlayerStatsDto, WoWMoPClassicPlayerStatsModel>().ReverseMap();
        CreateMap<WoWMidnightPlayerStatsDto, WoWMidnightPlayerStatsModel>().ReverseMap();
    }
}
