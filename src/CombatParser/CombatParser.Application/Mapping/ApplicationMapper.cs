using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Application.DTOs.Chart;
using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.Chart;
using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Application.Mapping;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<PlayerDto, Player>().ReverseMap();
        CreateMap<BossDto, Boss>().ReverseMap();
        CreateMap<BossMapDto, BossMap>().ReverseMap();
        CreateMap<CombatAbilityDto, CombatAbility>().ReverseMap();
        CreateMap<CombatLogDto, CombatLog>().ReverseMap();
        CreateMap<CombatDto, Combat>().ReverseMap();
        CreateMap<CombatPlayerDto, CombatPlayer>().ReverseMap();
        CreateMap<CombatPlayerPreAuraDto, CombatPlayerPreAura>().ReverseMap();
        CreateMap<Domain.DTOs.CombatPlayerPreAuraDto, CombatPlayerPreAuraDto>().ReverseMap();
        CreateMap<CombatPlayerAuraDto, CombatPlayerAura>().ReverseMap();
        CreateMap<CombatPlayerPositionDto, CombatPlayerPosition>().ReverseMap();
        CreateMap<SpecializationDto, Specialization>().ReverseMap();
        CreateMap<SpecializationScoreDto, SpecializationScore>().ReverseMap();
        CreateMap<BestSpecializationScoreDto, BestSpecializationScore>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneDto, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<CombatPlayerDeathDto, CombatPlayerDeath>().ReverseMap();
        CreateMap<CombatPlayerStatsDto, CombatPlayerStats>().ReverseMap();
        CreateMap<CombatTargetDto, CombatTarget>().ReverseMap();

        CreateMap<ChartGenericDto, ChartGeneric>().ReverseMap();
    }
}