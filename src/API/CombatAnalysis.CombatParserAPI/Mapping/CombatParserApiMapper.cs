using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Domain.EntityData;

namespace CombatAnalysis.CombatParserAPI.Mapping;

internal class CombatParserApiMapper : Profile
{
    public CombatParserApiMapper()
    {
        CreateMap<CombatDto, CombatModel>()
            .ForPath(dest => dest.Boss.Id,
               opt => opt.MapFrom(src => src.BossId));
        CreateMap<CombatModel, CombatDto>()
            .ForMember(dest => dest.BossId,
               opt => opt.MapFrom(src => src.Boss.Id));

        CreateMap<CombatPlayerDto, CombatPlayerModel>()
            .ForPath(dest => dest.Player.Id,
                opt => opt.MapFrom(src => src.PlayerId));
        CreateMap<CombatPlayerModel, CombatPlayerDto>()
            .ForMember(dest => dest.PlayerId,
               opt => opt.MapFrom(src => src.Player.Id));

        CreateMap<PlayerModel, PlayerDto>().ReverseMap();
        CreateMap<BossModel, Boss>().ReverseMap();
        CreateMap<BossModel, BossDto>().ReverseMap();
        CreateMap<CombatLogDto, CombatLogModel>().ReverseMap();
        CreateMap<CombatAuraDto, CombatAuraModel>().ReverseMap();
        CreateMap<CombatPlayerPositionDto, CombatPlayerPositionModel>().ReverseMap();
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

        CreateMap<PlayerDeath, CombatPlayerDeathModel>().ReverseMap();
        CreateMap<PlayerStats, CombatPlayerStatsModel>().ReverseMap();
        CreateMap<Combat, CombatModel>().ReverseMap();
        CreateMap<CombatPlayer, CombatPlayerModel>().ReverseMap();
        CreateMap<DamageDone, DamageDoneModel>().ReverseMap();
        CreateMap<DamageDoneGeneral, DamageDoneGeneralModel>().ReverseMap();
        CreateMap<HealDone, HealDoneModel>().ReverseMap();
        CreateMap<HealDoneGeneral, HealDoneGeneralModel>().ReverseMap();
        CreateMap<DamageTaken, DamageTakenModel>().ReverseMap();
        CreateMap<DamageTakenGeneral, DamageTakenGeneralModel>().ReverseMap();
        CreateMap<ResourceRecovery, ResourceRecoveryModel>().ReverseMap();
        CreateMap<ResourceRecoveryGeneral, ResourceRecoveryGeneralModel>().ReverseMap();

        CreateMap<CombatPlayerDeathDto, PlayerDeath>().ReverseMap();
        CreateMap<CombatPlayerStatsDto, PlayerStats>().ReverseMap();
        CreateMap<PlayerModel, Player>().ReverseMap();
        CreateMap<CombatDto, Combat>().ReverseMap();
        CreateMap<CombatModel, Combat>().ReverseMap();
        CreateMap<CombatAuraDto, CombatAura>().ReverseMap();
        CreateMap<CombatPlayerPositionDto, CombatPlayerPosition>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneDto, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>().ReverseMap();

        CreateMap<SpecializationScoreDto, SpecializationScore>().ReverseMap();

        CreateMap<DamageDoneData, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralData, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneData, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralData, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenData, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralData, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryData, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralData, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<CombatPlayerDeathData, PlayerDeath>().ReverseMap();
        CreateMap<CombatPlayerStatsData, CombatPlayerStatsModel>().ReverseMap();
        CreateMap<CombatPlayerPositionData, CombatPlayerPosition>().ReverseMap();
        CreateMap<SpecializationScoreData, SpecializationScoreModel>().ReverseMap();
    }
}
