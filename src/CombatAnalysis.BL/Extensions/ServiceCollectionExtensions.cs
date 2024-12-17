using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Services.Filters;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CombatParserBLDependencies(this IServiceCollection services, IConfiguration configuration, string connectionName, int commandTimeout)
    {
        services.CombatParserDALDependencies(configuration, connectionName, commandTimeout);

        services.AddScoped<ICombatTransactionService, CombatTransactionService>();

        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto>, PlayerInfoService<DamageDoneGeneralDto, DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoService<DamageDoneDto>, PlayerInfoService<DamageDoneDto, DamageDone>>();
        services.AddScoped<ICountService<DamageDoneDto>, CountService<DamageDoneDto, DamageDone>>();
        services.AddScoped<IGeneralFilterService<DamageDoneDto>, GeneralFilterService<DamageDoneDto, DamageDone>>();

        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto>, PlayerInfoService<HealDoneGeneralDto, HealDoneGeneral>>();
        services.AddScoped<IPlayerInfoService<HealDoneDto>, PlayerInfoService<HealDoneDto, HealDone>>();
        services.AddScoped<ICountService<HealDoneDto>, CountService<HealDoneDto, HealDone>>();
        services.AddScoped<IGeneralFilterService<HealDoneDto>, GeneralFilterService<HealDoneDto, HealDone>>();

        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto>, PlayerInfoService<DamageTakenGeneralDto, DamageTakenGeneral>>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto>, PlayerInfoService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<ICountService<DamageTakenDto>, CountService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<IGeneralFilterService<DamageTakenDto>, GeneralFilterService<DamageTakenDto, DamageTaken>>();

        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto>, PlayerInfoService<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto>, PlayerInfoService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<ICountService<ResourceRecoveryDto>, CountService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<IGeneralFilterService<ResourceRecoveryDto>, GeneralFilterService<ResourceRecoveryDto, ResourceRecovery>>();

        services.AddScoped<IPlayerInfoService<PlayerDeathDto>, PlayerInfoService<PlayerDeathDto, PlayerDeath>>();

        services.AddScoped<ISpecScoreService, SpecializationScoreService>();

        SetMutationServices(services);
        SetQueryServices(services);
    }

    private static void SetMutationServices(IServiceCollection services)
    {
        services.AddScoped<IMutationService<CombatLogDto>, CombatLogService>();
        services.AddScoped<IMutationService<CombatLogByUserDto>, CombatLogByUserService>();
        services.AddScoped<IMutationService<CombatDto>, CombatService>();
        services.AddScoped<IMutationService<CombatPlayerDto>, CombatPlayerService>();
        services.AddScoped<IMutationService<CombatPlayerPositionDto>, CombatPlayerPositionService>();
        services.AddScoped<IMutationService<PlayerDeathDto>, PlayerDeathService>();
        services.AddScoped<IMutationService<CombatAuraDto>, CombatAuraService>();
        services.AddScoped<IMutationService<PlayerParseInfoDto>, PlayerParseInfoService>();

        services.AddScoped<IMutationService<DamageDoneDto>, DamageDoneService>();
        services.AddScoped<IMutationService<DamageDoneGeneralDto>, DamageDoneGeneralService>();
        services.AddScoped<IMutationService<HealDoneDto>, HealDoneService>();
        services.AddScoped<IMutationService<HealDoneGeneralDto>, HealDoneGeneralService>();
        services.AddScoped<IMutationService<DamageTakenDto>, DamageTakenService>();
        services.AddScoped<IMutationService<DamageTakenGeneralDto>, DamageTakenGeneralService>();
        services.AddScoped<IMutationService<ResourceRecoveryDto>, ResourceRecoveryService>();
        services.AddScoped<IMutationService<ResourceRecoveryGeneralDto>, ResourceRecoveryGeneralService>();

        services.AddScoped<IMutationService<SpecializationScoreDto>, SpecializationScoreService>();
    }

    private static void SetQueryServices(IServiceCollection services)
    {
        services.AddScoped<IQueryService<CombatLogDto>, CombatLogService>();
        services.AddScoped<IQueryService<CombatLogByUserDto>, CombatLogByUserService>();
        services.AddScoped<IQueryService<CombatDto>, CombatService>();
        services.AddScoped<IQueryService<CombatPlayerDto>, CombatPlayerService>();
        services.AddScoped<IQueryService<CombatPlayerPositionDto>, CombatPlayerPositionService>();
        services.AddScoped<IQueryService<PlayerDeathDto>, PlayerDeathService>();
        services.AddScoped<IQueryService<CombatAuraDto>, CombatAuraService>();
        services.AddScoped<IQueryService<PlayerParseInfoDto>, PlayerParseInfoService>();

        services.AddScoped<IQueryService<DamageDoneDto>, DamageDoneService>();
        services.AddScoped<IQueryService<DamageDoneGeneralDto>, DamageDoneGeneralService>();
        services.AddScoped<IQueryService<HealDoneDto>, HealDoneService>();
        services.AddScoped<IQueryService<HealDoneGeneralDto>, HealDoneGeneralService>();
        services.AddScoped<IQueryService<DamageTakenDto>, DamageTakenService>();
        services.AddScoped<IQueryService<DamageTakenGeneralDto>, DamageTakenGeneralService>();
        services.AddScoped<IQueryService<ResourceRecoveryDto>, ResourceRecoveryService>();
        services.AddScoped<IQueryService<ResourceRecoveryGeneralDto>, ResourceRecoveryGeneralService>();

        services.AddScoped<IQueryService<SpecializationScoreDto>, SpecializationScoreService>();
    }
}
