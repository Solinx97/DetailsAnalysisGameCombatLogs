using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Application.DTOs.Chart;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Application.DTOs.WoWMidnight;
using CombatParser.Application.DTOs.WoWMoPClassic;
using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.Chart;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.Dashboard;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;

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
        CreateMap<CombatUnitDto, CombatUnit>().ReverseMap();
        CreateMap<UnitHealthDto, UnitHealth>().ReverseMap();
        CreateMap<UnitPositionDto, UnitPosition>().ReverseMap();
        CreateMap<UnitCastDto, UnitCast>().ReverseMap();
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

        CreateMap<Interfaces.IPlayerStatsDto, Domain.Interfaces.IPlayerStats>()
            .Include<WoWMoPClassicPlayerStatsDto, WoWMoPClassicPlayerStats>()
            .Include<WoWMidnightPlayerStatsDto, WoWMidnightPlayerStats>().ReverseMap();

        CreateMap<WoWMoPClassicPlayerStatsDto, WoWMoPClassicPlayerStats>().ReverseMap();
        CreateMap<WoWMidnightPlayerStatsDto, WoWMidnightPlayerStats>().ReverseMap();

        CreateMap<ChartGenericDto, ChartGeneric>().ReverseMap();
        CreateMap<DashboardDto, Dashboard>().ReverseMap();
    }
}